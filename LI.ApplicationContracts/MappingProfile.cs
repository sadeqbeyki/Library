using AutoMapper;
using LI.ApplicationContracts.RoleContracts;
using LI.ApplicationContracts.UserContracts;
using LI.Domain.RoleAgg;
using LI.Domain.UserAgg;

namespace LI.ApplicationContracts;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UpdateUserDto, User>().ReverseMap();
        CreateMap<RoleDto, Role>().ReverseMap();
    }
}
