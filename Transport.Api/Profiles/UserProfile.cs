using AutoMapper;
using Transport.Application.Contract.Dtos;
using Transport.Domain;

namespace Transport.Api.Profiles
{
    public class UserProfile : Profile
    {
        public UserProfile()
        {
            CreateMap<User, UserDto>()
                .ForMember(dest => dest.Login, opt => opt.MapFrom(src => src.Login.Name))
                .ForMember(dest => dest.Name, opt => opt.MapFrom(src => src.Name.Name))
                .ForMember(dest => dest.Company, opt => opt.MapFrom(src => src.Company.Name))
                .ForMember(dest => dest.NumberOfFollowers, opt => opt.MapFrom(src => src.NumberOfFollowers))
                .ForMember(dest => dest.NumberOfPublicRepositories, opt => opt.MapFrom(src => src.NumberOfPublicRepositories));
        }
    }
}
