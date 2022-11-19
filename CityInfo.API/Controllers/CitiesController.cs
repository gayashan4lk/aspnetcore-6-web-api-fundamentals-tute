using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;

        public CitiesController(ILogger<CitiesController> logger)
        {
            this.logger = logger;
        }

        [HttpGet()]
        public ActionResult GetCities()
        {
            var cities = CitiesDataStore.Current.Cities;
            return Ok(cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if (city == null)
            {
                logger.LogInformation($"City with id {id} is not found.");
                return NotFound();
            }

            return Ok(city);
        }
    } 
}
