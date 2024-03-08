using AutoMapper;
using Library.Application.DTOs.BookCategory;
using Library.Application.DTOs.Lends;
using Library.Domain.Entities.BookCategoryAgg;
using Library.Domain.Entities.LendAgg;

namespace Library.Application.Helper;

public class BookMapProfile : Profile
{
    public BookMapProfile()
    {
        CreateMap<BookCategoryDto, BookCategory>().ReverseMap();

        CreateMap<Lend, LendDto>()
            .ForMember(dest => dest.Id, opt => opt.MapFrom(src => src.Id))
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => DateTime.Now))
            .ForMember(dest => dest.ReturnEmployeeID, opt => opt.MapFrom(src => src.ReturnEmployeeID))
            .ReverseMap();

        CreateMap<Lend, LendDto>().ReverseMap();
    }
}
