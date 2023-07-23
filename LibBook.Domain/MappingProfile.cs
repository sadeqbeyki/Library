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
        CreateMap<BorrowDto, Borrow>().ReverseMap();
        //CreateMap<UpdateUserDto, User>().ReverseMap();
        //CreateMap<RoleDto, Role>().ReverseMap();
    }
}
