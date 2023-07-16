using AutoMapper;
using LendBook.ApplicationContract.Lend;
using LendBook.Domain.LendAgg;

namespace LendBook.Domain;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<LendDto, Lend>().ReverseMap();
    }
}
