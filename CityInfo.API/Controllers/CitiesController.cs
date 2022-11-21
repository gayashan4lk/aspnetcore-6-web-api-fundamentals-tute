using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;
        private readonly CitiesDataStore citiesDataStore;
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;

        public CitiesController(ILogger<CitiesController> logger, CitiesDataStore citiesDataStore, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.citiesDataStore = citiesDataStore ?? throw new ArgumentNullException(nameof(citiesDataStore));
            this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities()
        {
            try
            {
                var cityEntities = await cityInfoRepository.GetCitiesAsync();
                //var results = new List<CityWithoutPointOfInterestDto>();
                /*foreach (var cityEntity in cityEntities)
                {
                    results.Add(new CityWithoutPointOfInterestDto
                    {
                        Id = cityEntity.CityId,
                        Name = cityEntity.Name,
                        Description = cityEntity.Description,
                    });
                }*/
                return Ok(mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities));
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
