using AutoMapper;
using CityInfo.API.Entities;
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
        public async Task<ActionResult<PointOfInterestDto>> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            if (!await cityInfoRepository.IsCityExistAsync(cityId)) return NotFound();
            var pointOfInterstEntity = mapper.Map<PointOfInterest>(pointOfInterestForCreation);
            await cityInfoRepository.AddPointOfInterestForCityAsync(cityId, pointOfInterstEntity);
            await cityInfoRepository.SaveChangesAsync();

            // Any of two ways can be used to return the created point of interest
            // Method 1
            var createdPointOfInterest = mapper.Map<PointOfInterestDto>(pointOfInterstEntity);
            return CreatedAtRoute("GetPointOfInterest",
                new
                {
                    cityId = cityId,
                    pointId = createdPointOfInterest.Id,
                },
                createdPointOfInterest);

            // Method 2
            /*var createdPointOfInterest = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterstEntity.PointOfInterestId);
            return Ok(mapper.Map<PointOfInterestDto>(createdPointOfInterest));*/
        }

        [HttpPut("{PointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterstForUpdate)
        {
            if (!(await cityInfoRepository.IsCityExistAsync(cityId) 
                || await cityInfoRepository.IsPointOfInterestExistAsync(pointOfInterestId))) 
                return NotFound();

            var pointOfInterestEntity = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            // Automapper replaces values of destination object (pointOfInterestEntity) with values of source object (pointOfInterstForUpdate).
            mapper.Map(pointOfInterstForUpdate, pointOfInterestEntity);

            // Because pointOfInterestEntity is tracked by DbContext, after call SaveChangesAsync() changes to the db is persisted.
            await cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpPatch("{PointOfInterestId}")]
        public async Task<ActionResult> UpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            if (!(await cityInfoRepository.IsCityExistAsync(cityId)
                || await cityInfoRepository.IsPointOfInterestExistAsync(pointOfInterestId)))
                return NotFound();

            var pointOfInterestEntity = await cityInfoRepository.GetPointOfInterestForCityAsync(cityId, pointOfInterestId);

            var pointOfInterestToPatch = mapper.Map<PointOfInterestForUpdateDto>(pointOfInterestEntity);

            patchDocument.ApplyTo(pointOfInterestToPatch, ModelState);

            if (!ModelState.IsValid) 
                return BadRequest(ModelState);

            if (!TryValidateModel(pointOfInterestToPatch)) 
                return BadRequest(ModelState);

            mapper.Map(pointOfInterestToPatch, pointOfInterestEntity);

            await cityInfoRepository.SaveChangesAsync();

            return NoContent();
        }

        [HttpDelete("{PointOfInterestId}")]
        public async Task<ActionResult> DeletePointOfInterest (int cityId, int pointOfInterestId)
        {
            if (!(await cityInfoRepository.IsCityExistAsync(cityId)
                || await cityInfoRepository.IsPointOfInterestExistAsync(pointOfInterestId)))
                return NotFound();

            var pointOfInterestToDelete = await cityInfoRepository.GetPointOfInterestForCityAsync (cityId, pointOfInterestId);

            if (pointOfInterestToDelete != null)
            {
                cityInfoRepository.DeletePointOfInterest(pointOfInterestToDelete);

                await cityInfoRepository.SaveChangesAsync();

                mailService.Send("Point of interest was deleted", $"Point of Interest {pointOfInterestToDelete.Name} with id: {pointOfInterestToDelete.PointOfInterestId} was deleted.");
            }

            return NoContent();
        }
    }
}
