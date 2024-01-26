using AutoMapper;
using Identity.Application.DTOs.Role;
using Identity.Application.DTOs.User;
using Identity.Domain.Entities.Role;
using Identity.Domain.Entities.User;

namespace Identity.Application;

public class IdentityMapProfile : Profile
{
    public IdentityMapProfile()
    {
        CreateMap<UserDetailsDto, UpdateUserDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.FirstName, opt => opt.MapFrom(src => src.FirstName))
            .ForMember(dest => dest.LastName, opt => opt.MapFrom(src => src.LastName))
            .ForMember(dest => dest.BirthDate, opt => opt.MapFrom(src => src.BirthDate))
            .ForMember(dest => dest.UserName, opt => opt.MapFrom(src => src.UserName))
            .ForMember(dest => dest.Email, opt => opt.MapFrom(src => src.Email))
            .ForMember(dest => dest.PhoneNumber, opt => opt.MapFrom(src => src.PhoneNumber))
            .ForMember(dest => dest.Roles, opt => opt.MapFrom(src => src.Roles));

        CreateMap<UserDetailsDto, ApplicationUser>().ReverseMap();
        CreateMap<CreateUserDto, ApplicationUser>().ReverseMap();
        CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
        CreateMap<UserRolesDto, ApplicationUser>().ReverseMap();

        CreateMap<RoleDto, ApplicationRole>().ReverseMap();
    }
}
