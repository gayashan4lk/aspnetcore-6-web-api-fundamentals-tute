using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The best city in usa"
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Jakartha",
                    Description = "Capital of Indunisia",
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Tokyo",
                    Description = "A Nice city of Japan"
                }
            };
        }
    }
}
