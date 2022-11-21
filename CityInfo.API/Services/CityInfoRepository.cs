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
            return await context.Cities.OrderBy(c => c.CityId).ToListAsync();
        }

        public async Task<City?> GetCity(int cityId, bool includePointsOfInterest)
        {
            if(includePointsOfInterest) return await context.Cities
                    .Include(c => c.PointsOfInterest)
                    .Where(c => c.CityId == cityId)
                    .FirstOrDefaultAsync();

            return await context.Cities.Where(c => c.CityId == cityId).FirstOrDefaultAsync();
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
    }
}
