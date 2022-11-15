using CityInfo.API.Models;
using Microsoft.AspNetCore.Mvc;

namespace CityInfo.API.Controllers
{
    [ApiController]
    [Route("api/cities")]
    public class CitiesController : ControllerBase
    {
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
                return NotFound();

            return Ok(city);
        }

        [HttpPost("{data}")]
        public JsonResult GetSomethingelse(string data)
        {
            return new JsonResult(
                new List<object>
                {
                    new {Content= data},
                    new {Content= "hohoho"}
                });
        }
    }
}
