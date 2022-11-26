using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Text.Json;

namespace CityInfo.API.Controllers
{
    [ApiController]
    //[Authorize]
    [ApiVersion("1.0")]
    [ApiVersion("2.0")]
    [Route("api/v{version:apiVersion}/cities")]
    public class CitiesController : ControllerBase
    {
        private readonly ILogger<CitiesController> logger;
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;
        const int maxCitiesPageSize = 10;

        public CitiesController(ILogger<CitiesController> logger, ICityInfoRepository cityInfoRepository, IMapper mapper)
        {
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
            this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this.mapper = mapper;
        }

        [HttpGet()]
        public async Task<ActionResult<IEnumerable<CityWithoutPointOfInterestDto>>> GetCities(string? name, string?searchQuery, int pageSize = 10, int pageNumber = 1)
        {
            try
            {
                if (pageSize > maxCitiesPageSize)
                    pageSize = maxCitiesPageSize;
                
                var (cityEntities, paginationMetadata) = await cityInfoRepository.GetCitiesAsync(name, searchQuery, pageSize, pageNumber);

                Response.Headers.Add("X-Pagination", JsonSerializer.Serialize(paginationMetadata));

                return Ok(mapper.Map<IEnumerable<CityWithoutPointOfInterestDto>>(cityEntities));
            }
            catch (Exception ex)
            {
                logger.LogCritical("Exception occured while getting cities.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }

        /// <summary>
        /// Get a city by id
        /// </summary>
        /// <param name="id">Id of the city to get</param>
        /// <param name="isPointsOfInterestIncluded">Whether or not to include the points of interest</param>
        /// <returns>IActionResult</returns>
        /// <response code="200">OK : Returns the requested city</response>
        /// <response code="500">InternalServerError : Exception occured while getting a city</response>
        [HttpGet("{id}")]
        [ProducesResponseType(StatusCodes.Status200OK)]
        [ProducesResponseType(StatusCodes.Status404NotFound)]
        [ProducesResponseType(StatusCodes.Status400BadRequest)]
        [ProducesResponseType(StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetCity(int id, bool isPointsOfInterestIncluded = false)
        {
            try
            {
                var cityEntity = await cityInfoRepository.GetCityAsync(id, isPointsOfInterestIncluded);

                if (cityEntity == null) return NotFound();

                if (isPointsOfInterestIncluded) return Ok(mapper.Map<CityDto>(cityEntity));

                return Ok(mapper.Map<CityWithoutPointOfInterestDto>(cityEntity));
            }
            catch (Exception ex)
            {
                logger.LogCritical("Exception occured while getting a city.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }
    } 
}
