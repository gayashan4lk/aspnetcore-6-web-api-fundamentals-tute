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
            List<CityDto> cities = CitiesDataStore.Current.Cities;
            return new JsonResult(cities);
                
        }

        [HttpGet("{id}")]
        public JsonResult GetCity(int id)
        {
            CityDto? city = CitiesDataStore.Current.Cities.FirstOrDefault(c => c.Id == id);
            return new JsonResult(city);
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
