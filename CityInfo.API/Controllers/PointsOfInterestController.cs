using CityInfo.API.Models;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [Route("api/cities/{cityId}/pointsofinterest")]
    [ApiController]
    public class PointsOfInterestController : ControllerBase
    {
        [HttpGet()]
        public ActionResult<List<PointOfInterestDto>> GetPointsOfInterest(int cityId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();
            var points = city.PointsOfInterest;
            if (points == null)
                return NotFound();
            return Ok(points);
        }
        
        [HttpGet("{pointId}", Name = "getPointOfInterest")]
        public ActionResult<PointOfInterestDto> GetPointOfInterest(int cityId, int pointId)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

            var point = city.PointsOfInterest.FirstOrDefault(x => x.Id == pointId);
            if (point == null)
                return NotFound();
            return Ok(point);
        }

        [HttpPost]
        public ActionResult<PointOfInterestDto> CreatePointOfInterest(int cityId, PointOfInterestForCreationDto pointOfInterestForCreation)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(x => x.Id == cityId);
            if (city == null)
                return NotFound();

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
                }
                ,newPointOfInterest);
        }
    }
}
