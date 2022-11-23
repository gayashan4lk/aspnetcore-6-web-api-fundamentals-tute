using AutoMapper;

namespace CityInfo.API.Profiles
{
    public class CityProfile : Profile
    {
        public CityProfile()
        {
            CreateMap<Entities.City, Models.CityWithoutPointOfInterestDto>()
                .ForMember(dest=> dest.Id, opt => opt.MapFrom(src => src.CityId));
            CreateMap<Entities.City, Models.CityDto>()
                .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.CityId));
        }
    }
}
