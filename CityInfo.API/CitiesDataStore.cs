using CityInfo.API.Models;

namespace CityInfo.API
{
    public class CitiesDataStore
    {
        public List<CityDto> Cities { get; set; }
        //public static CitiesDataStore Current { get; } = new CitiesDataStore();

        public CitiesDataStore()
        {
            Cities = new List<CityDto>()
            {
                new CityDto()
                {
                    Id = 1,
                    Name = "New York City",
                    Description = "The best city in usa",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central park",
                            Description = "There are so many trees"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Market",
                            Description = "There are so many people"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 2,
                    Name = "Jakartha",
                    Description = "Capital of Indunisia",
                    PointsOfInterest = new List<PointOfInterestDto>()
                    {
                        new PointOfInterestDto()
                        {
                            Id = 1,
                            Name = "Central park",
                            Description = "There are so many trees"
                        },
                        new PointOfInterestDto()
                        {
                            Id = 2,
                            Name = "Central Market",
                            Description = "There are so many people"
                        }
                    }
                },
                new CityDto()
                {
                    Id = 3,
                    Name = "Tokyo",
                    Description = "A Nice city of Japan",
                }
            };
        }
    }
}
