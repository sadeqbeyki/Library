using AutoMapper;
using LibIdentity.Domain.RoleAgg;
using LibIdentity.Domain.UserAgg;
using LibIdentity.DomainContracts.RoleContracts;
using LibIdentity.DomainContracts.UserContracts;

namespace LibIdentity.DomainContracts;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<UserDto, User>().ReverseMap();
        CreateMap<UpdateUserDto, User>().ReverseMap();
        CreateMap<RoleDto, Role>().ReverseMap();
    }
}
