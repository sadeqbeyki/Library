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

        CreateMap<Borrow, LoanDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ReturnEmployeeId, opt => opt.MapFrom(src => src.ReturnEmployeeID))
            .ReverseMap();

        CreateMap<Borrow, LoanDto>().ReverseMap();
    }
}
