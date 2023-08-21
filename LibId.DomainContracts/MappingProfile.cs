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
        CreateMap<CreateUserViewModel, UserIdentity>().ReverseMap();
        CreateMap<UpdateUserViewModel, UserIdentity>().ReverseMap();
        CreateMap<UserRolesViewModel, UserIdentity>().ReverseMap();
        CreateMap<RoleDto, RoleIdentity>().ReverseMap();
    }
}
