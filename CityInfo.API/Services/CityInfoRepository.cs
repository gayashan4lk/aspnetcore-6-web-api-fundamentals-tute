using CityInfo.API.DbContexts;
using CityInfo.API.Entities;
using Microsoft.EntityFrameworkCore;

namespace CityInfo.API.Services
{
    public class CityInfoRepository : ICityInfoRepository
    {
        private readonly CityInfoContext context;

        public CityInfoRepository(CityInfoContext context)
        {
            this.context = context ?? throw new ArgumentNullException(nameof(context));
        }

        public async Task<IEnumerable<City>> GetCitiesAsync()
        {
            return await context.Cities.OrderBy(c => c.Name).ToListAsync();
        }

        public async Task<IEnumerable<City>> GetCitiesAsync(string? name)
        {
            if (string.IsNullOrEmpty(name))
                return await GetCitiesAsync();

            name = name.Trim();
            return await context.Cities
                .Where(c => c.Name == name)
                .OrderBy(c => c.Name)
                .ToListAsync();
        }

        public async Task<City?> GetCityAsync(int cityId, bool includePointsOfInterest)
        {
            if (includePointsOfInterest) return await context.Cities
                     .Include(c => c.PointsOfInterest)
                     .Where(c => c.CityId == cityId)
                     .FirstOrDefaultAsync();

            return await context.Cities.Where(c => c.CityId == cityId).FirstOrDefaultAsync();
        }

        public async Task<bool> IsCityExistAsync(int cityId)
        {
            return await context.Cities.AnyAsync(c => c.CityId == cityId);
        }

        public async Task<bool> IsPointOfInterestExistAsync(int pointOfInterestId)
        {
            return await context.PointsOfInterest.AnyAsync(p => p.PointOfInterestId == pointOfInterestId);
        }

        public async Task<IEnumerable<PointOfInterest>> GetPointsOfInterestForCityAsync(int cityId)
        {
            return await context.PointsOfInterest
                .Where(p => p.CityId == cityId)
                .OrderBy(p => p.CityId)
                .ToListAsync();
        }

        public async Task<PointOfInterest?> GetPointOfInterestForCityAsync(int cityId, int pointOfInterestId)
        {
            return await context.PointsOfInterest
                .Where(p => p.PointOfInterestId == pointOfInterestId && p.CityId == cityId)
                .FirstOrDefaultAsync();
        }

        public async Task AddPointOfInterestForCityAsync(int cityId, PointOfInterest pointOfInterest)
        {
            var cityEntity = await GetCityAsync(cityId, false);
            if (cityEntity != null)
            {
                cityEntity.PointsOfInterest.Add(pointOfInterest);
            }
        }

        public async Task<bool> SaveChangesAsync()
        {
            return await context.SaveChangesAsync() >= 0 ;
        }

        public void DeletePointOfInterest(PointOfInterest pointOfInterest)
        {
            context.PointsOfInterest.Remove(pointOfInterest);
        }
    }
}
