using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        [HttpGet()]
        public JsonResult GetCities()
        {
            return new JsonResult(
                new List<object>
                {
                    new { Id = 1, Name = "New York City"},
                    new { Id = 2, Name = "Jakarta"}
                });
        }

        [HttpPost()]
        public JsonResult GetSomethingelse()
        {
            return new JsonResult(
                new List<object>
                {
                    new {Content= "ooohh"},
                    new {Content= "hohoho"}
                });
        }
    }
}
