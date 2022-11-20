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
            try
            {
                var cities = CitiesDataStore.Current.Cities;
                return Ok(cities);
            }
            catch (Exception ex)
            {
                logger.LogCritical("Exception occured while getting cities.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            try
            {
                var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

                if (city == null)
                {
                    logger.LogInformation($"City with id {id} is not found.");
                    return NotFound();
                }

                return Ok(city);
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception occured while getting the city with id : {id}.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }
    } 
}
