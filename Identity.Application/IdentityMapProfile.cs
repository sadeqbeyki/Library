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
        CreateMap<UserDetailsDto, ApplicationUser>().ReverseMap();
        CreateMap<CreateUserDto, ApplicationUser>().ReverseMap();
        CreateMap<UpdateUserDto, ApplicationUser>().ReverseMap();
        CreateMap<UserRolesDto, ApplicationUser>().ReverseMap();
        CreateMap<RoleDto, ApplicationRole>().ReverseMap();
    }
}
