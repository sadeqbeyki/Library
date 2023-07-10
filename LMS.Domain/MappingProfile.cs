using AutoMapper;
using LMS.Contracts.BookCategoryContract;
using LMS.Contracts.Loan;
using LMS.Domain.BookCategoryAgg;
using LMS.Domain.LoanAgg;

namespace LMS.Domain;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCategoryDto, BookCategory>().ReverseMap();
        CreateMap<LoanDto, Loan>().ReverseMap();
        //CreateMap<UpdateUserDto, User>().ReverseMap();
        //CreateMap<RoleDto, Role>().ReverseMap();
    }
}
