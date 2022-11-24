using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class PointOfInterestProfile : Profile
    {
        public PointOfInterestProfile()
        {
            CreateMap<Entities.PointOfInterest, Models.PointOfInterestDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.PointOfInterestId));
            CreateMap<Models.PointOfInterestForCreationDto, Entities.PointOfInterest>();
        }
    }
}
