using AutoMapper;
using LibBook.Domain.BookCategoryAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCategoryDto, BookCategory>().ReverseMap();

        CreateMap<LoanDto, Borrow>()
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.BorrowDate));
        CreateMap<Borrow, LoanDto>()
            .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.CreationDate));

        //CreateMap<UpdateUserDto, User>().ReverseMap();
        //CreateMap<RoleDto, Role>().ReverseMap();
    }
}
