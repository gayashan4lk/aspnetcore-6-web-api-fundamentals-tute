using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.DbContexts
{
    public class CityInfoContext : DbContext
    {
        public DbSet<City> Cities { get; set; } = null!;
        public DbSet<PointOfInterest> PointsOfInterest { get; set; } = null!;

        public CityInfoContext(DbContextOptions<CityInfoContext> options) : base(options)
        {
        }

        protected override void OnModelCreating(ModelBuilder modelBuilder)
        {
            modelBuilder.Entity<City>()
                .HasData(
                new City("New York City")
                {
                    CityId = 1,
                    Description = "The best city in usa",
                },
                new City("Jakartha")
                {
                    CityId = 2,
                    Description = "Capital of Indunisia",
                },
                new City("Tokyo")
                {
                    CityId = 3,
                    Description = "A Nice city of Japan"
                });

            modelBuilder.Entity<PointOfInterest>()
                .HasData(
                new PointOfInterest("Central park")
                {
                    PointOfInterestId = 1,
                    Description = "There are so many trees",
                    CityId = 1,
                },
                new PointOfInterest("Central Market")
                {
                    PointOfInterestId = 2,
                    Description = "There are so many people",
                    CityId = 1,
                },
                new PointOfInterest("Transport Center")
                {
                    PointOfInterestId = 3,
                    Description = "There are so many buses",
                    CityId = 2,
                },
                new PointOfInterest("Fish Market")
                {
                    PointOfInterestId = 4,
                    Description = "There are so many sea food",
                    CityId = 3,
                },
                new PointOfInterest("Harbor")
                {
                    PointOfInterestId = 5,
                    Description = "There are so many boats",
                    CityId = 2,
                });

            base.OnModelCreating(modelBuilder);
        }

        /*protected override void OnConfiguring(DbContextOptionsBuilder optionsBuilder)
        {
            optionsBuilder.UseSqlite("connection string");
            base.OnConfiguring(optionsBuilder);
        }*/
    }
}
