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

        public PointsOfInterestController(IMailService mailService)
        {
            this.mailService = mailService;
        }

        [HttpGet()]
        public ActionResult<List<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var points = city.PointsOfInterest;
            if (points == null) return NotFound();

            return Ok(points);
        }
        
        [HttpGet("{pointId}", Name = "getPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var point = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointId);
            if (point == null) return NotFound();

            return Ok(point);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var maxPointOfInterest = CitiesDataStore.Current.Cities.SelectMany(p => p.PointsOfInterest).Max(x => x.Id);

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
                newPointOfInterest);
        }

        [HttpPut("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, PointOfInterestForUpdateDto pointOfInterst)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointFromStore == null) return NotFound();

            pointFromStore.Name = pointOfInterst.Name;
            pointFromStore.Description = pointOfInterst.Description;

            return NoContent();
        }

        [HttpPatch("{PointOfInterestId}")]
        public ActionResult UpdatePointOfInterest(int cityId, int pointOfInterestId, JsonPatchDocument<PointOfInterestForUpdateDto> patchDocument)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
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

            return NoContent();
        }

        [HttpDelete("{PointOfInterestId}")]
        public ActionResult DeletePointOfInterest (int cityId, int pointOfInterestId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null) return NotFound();

            var pointFromStore = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointOfInterestId);
            if (pointFromStore == null) return NotFound();

            city.PointsOfInterest.Remove(pointFromStore);
            mailService.Send("Point of interest was deleted", $"Point of Interest {pointFromStore.Name}, id: {pointFromStore.Id} which was in city {city.Name}, id: {city.Id} was deleted.");

            return NoContent();
        }
    }
}
