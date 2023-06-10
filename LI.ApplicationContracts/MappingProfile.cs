using AutoMapper;
using LI.ApplicationContracts.UserContracts;
using LI.Domain.UserAgg;

namespace LI.ApplicationContracts;

public class MappingProfile:Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UpdateUserDto, User>().ReverseMap();
    }
}
