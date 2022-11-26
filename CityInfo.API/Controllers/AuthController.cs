using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using Microsoft.IdentityModel.Tokens;
using System.IdentityModel.Tokens.Jwt;
using System.Security.Claims;
using System.Text;

namespace CityInfo.API.Controllers
{
    [Route("api/auth")]
    [ApiController]
    public class AuthController : ControllerBase
    {
        private readonly IConfiguration configuration;

        // AuthRequestBody class is not used outside this class. So we can scope it to this namespace.
        public class AuthRequestBody
        {
            public string? UserName { get; set; }
            public string? Password { get; set; }
        }

        public class CityInfoUser
        {
            public int UserId { get; set; }
            public string UserName { get; set; }
            public string FirstName { get; set; }
            public string LastName { get; set; }
            public string City { get; set; }

            public CityInfoUser(int userId, string userName, string firstName, string lastName, string city)
            {
                UserId = userId;
                UserName = userName;
                FirstName = firstName;
                LastName = lastName;
                City = city;
            }
        }

        public AuthController(IConfiguration configuration)
        {
            this.configuration = configuration ?? throw new ArgumentNullException(nameof(configuration));
        }


        [HttpPost("authenticate")]
        public ActionResult<string> Authenticate(AuthRequestBody authRequestBody)
        {
            // Step 1. Validate the username / password
            var user = ValidateUserCredentials(authRequestBody.UserName, authRequestBody.Password);

            // If there is no user, return response - Unauthorized 
            if (user == null)
                return Unauthorized();

            // Step 2. Create a token
            var key = new SymmetricSecurityKey(Encoding.ASCII.GetBytes(configuration["Authentication:Secret"]));

            // Create Signing Credentials
            var signature = new SigningCredentials(key, SecurityAlgorithms.HmacSha256);

            // Create claims
            var claims = new List<Claim>();
            claims.Add(new Claim("sub", user.UserId.ToString()));
            claims.Add(new Claim("given_name", user.FirstName));
            claims.Add(new Claim("family_name", user.LastName));
            claims.Add(new Claim("city", user.City));

            // Create the JWT Security Token
            var jwtToken = new JwtSecurityToken(
                configuration["Authentication:Issuer"],
                configuration["Authentication:Audience"],
                claims,
                DateTime.UtcNow,
                DateTime.UtcNow.AddHours(1),
                signature);

            var tokenToReturn = new JwtSecurityTokenHandler().WriteToken(jwtToken);

            return Ok(tokenToReturn);
        }

        private CityInfoUser ValidateUserCredentials(string? userName, string? password)
        {
            /*  We don't have a user DB or table.
                If you have, check the passed username / password against what's stored in the database.
                For demo purposes, we assume the credentials are valid, and return a user detials.
            */
            // return a new CityInfoUser (Values would normally come for your user database/table)
            return new CityInfoUser(1, userName ?? "", "Joey", "Tribbiani", "Colombo");
        }
    }
}
