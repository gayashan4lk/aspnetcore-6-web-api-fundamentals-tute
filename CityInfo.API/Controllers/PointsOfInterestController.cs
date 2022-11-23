using AutoMapper;
using CityInfo.API.Models;
using CityInfo.API.Services;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.JsonPatch;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        private readonly IMailService mailService;
        private readonly ICityInfoRepository cityInfoRepository;
        private readonly IMapper mapper;
        private readonly ILogger<PointsOfInterestController> logger;

        public PointsOfInterestController(IMailService mailService, ICityInfoRepository cityInfoRepository, IMapper mapper, ILogger<PointsOfInterestController> logger)
        {
            this.mailService = mailService ?? throw new ArgumentNullException(nameof(mailService));
            this.cityInfoRepository = cityInfoRepository ?? throw new ArgumentNullException(nameof(cityInfoRepository));
            this.mapper = mapper ?? throw new ArgumentNullException(nameof(mapper));
            this.logger = logger ?? throw new ArgumentNullException(nameof(logger));
        }

        [HttpGet()]
        public async Task<ActionResult<List<PointOfInterestDto>>> GetPointsOfInterest(int cityId)
        {
            try
            {
                if (!await cityInfoRepository.IsCityExistAsync(cityId))
                {
                    logger.LogInformation($"City with id : {cityId} was not found when accessing points of interest.");
                    return NotFound();
                }
                
                var pointOfInterestEntities = await cityInfoRepository.GetPointsOfInterestForCityAsync(cityId);

                return Ok(mapper.Map<IEnumerable<PointOfInterestDto>>(pointOfInterestEntities));
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception occured while getting points of interest for city with Id : {cityId}.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }

        [HttpGet("{pointId}", Name = "getPointOfInterest")]
        public async Task<ActionResult<PointOfInterestDto>> GetPointOfInterest(int cityId, int pointId)
        {
            try
            {
                if (!await cityInfoRepository.IsCityExistAsync(cityId))
                {
                    logger.LogInformation($"City with id : {cityId} was not found when accessing points of interest.");
                    return NotFound();
                }

                var pointOfInterestEntity = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointId);

                if (pointOfInterestEntity == null)
                {
                    logger.LogInformation($"Point of interest with id : {pointId} for City with id : {cityId} was not found when accessing points of interest.");
                    return NotFound();
                }

                return Ok(mapper.Map<PointOfInterestDto>(pointOfInterestEntity));
            }
            catch (Exception ex)
            {
                logger.LogCritical($"Exception occured while getting point of interest for city with Id : {cityId}.", ex);
                return StatusCode(500, "A server error occured while handling your request.");
            }
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            /*var city = citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var maxPointOfInterest = citiesDataStore.Cities.SelectMany(p => p.PointsOfInterest).Max(x => x.Id);

            var newPointOfInterest = new PointOfInterestDto
            {
                Id = ++maxPointOfInterest,
                Name = pointOfInterestForCreation.Name,
                Description = pointOfInterestForCreation.Description
            };

            return CreatedAtRoute("getPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointId = newPointOfInterest.Id,
                },
                newPointOfInterest);*/
            return Ok(new PointOfInterestDto());
        }

        [HttpPut("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterst)
        {
            /*var city = citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointFromStore == null) return NotFound();

            pointFromStore.Name = pointOfInterst.Name;
            pointFromStore.Description = pointOfInterst.Description;

            return NoContent();*/
            return Ok();
        }

        [HttpPatch("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            /*var city = citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointFromStore == null) return NotFound();

            var pointToPatch = new PointOfInterestForUpdateDto()
            {
                Name = pointFromStore.Name,
                Description = pointFromStore.Description,
            };

            patchDocument.ApplyTo(pointToPatch, ModelState);

            if (!ModelState.IsValid) return BadRequest(ModelState);

            if (!TryValidateModel(pointToPatch)) return BadRequest(ModelState);

            pointFromStore.Name = pointToPatch.Name;
            pointFromStore.Description = pointToPatch.Description;

            return NoContent();*/
            return Ok();
        }

        [HttpDelete("{PointOfInterestId}")]
        public ActionResult DeletePointOfInterest (int cityId, int pointOfInterestId)
        {
            /*var city = citiesDataStore.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointFromStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointFromStore);
            mailService.Send("Point of interest was deleted", $"Point of Interest {pointFromStore.Name}, id: {pointFromStore.Id} which was in city {city.Name}, id: {city.Id} was deleted.");

            return NoContent();*/
            return Ok();
        }
    }
}
