using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;
        private readonly CitiesDataStore citiesDataStore;

        public CitiesController(ILogger<CitiesController> logger, CitiesDataStore citiesDataStore)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
        }

        [HttpGet()]
        public ActionResult GetCities()
        {
            try
            {
                var cities = citiesDataStore.Cities;
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
                var city = citiesDataStore.Cities.FirstOrDefault(c => c.Id == id);

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
