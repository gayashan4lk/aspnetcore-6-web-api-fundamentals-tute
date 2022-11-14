using CityInfo.API.Models;
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
            var cities = new JsonResult(CitiesDataStore.Current.Cities);
            cities.StatusCode = 200;
            return new JsonResult(cities);
        }

        [HttpGet("{id}")]
        public ActionResult<CityDto> GetCity(int id)
        {
            var city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);

            if(city == null)
                return NotFound();

            return Ok(city);
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
