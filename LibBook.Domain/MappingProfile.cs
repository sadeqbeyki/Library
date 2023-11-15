using AutoMapper;
using LibBook.Domain.BookCategoryAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.DomainContracts.BookCategory;
using LibBook.DomainContracts.Borrow;

namespace LibBook.Domain;

public class MappingProfile : Profile
{
    //public MappingProfile()
    //{
    //    CreateMap<BookCategoryDto, BookCategory>().ReverseMap();

    //    CreateMap<LoanDto, Borrow>()
    //        .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.BorrowDate));
    //    CreateMap<Borrow, LoanDto>()
    //        .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.CreationDate));

    //    //CreateMap<UpdateUserDto, User>().ReverseMap();
    //    //CreateMap<RoleDto, Role>().ReverseMap();
    //}

        public MappingProfile()
    {
        CreateMap<BookCategoryDto, BookCategory>().ReverseMap();

        CreateMap<LoanDto, Borrow>()
            .ForMember(dest => dest.CreationDate, opt => opt.MapFrom(src => src.BorrowDate));

        CreateMap<Borrow, LoanDto>()
            .ForMember(dest => dest.BorrowDate, opt => opt.MapFrom(src => src.CreationDate))
            .ForMember(dest => dest.Id, opt => opt.Ignore());
            //.ForMember(dest => dest.IsApproved, opt => opt.Ignore())
            //.ForMember(dest => dest.IsReturned, opt => opt.Ignore());

        // Add explicit mapping for Borrow to LoanDto
        CreateMap<Borrow, LoanDto>()
            .ForMember(dest => dest.MemberId, opt => opt.MapFrom(src => src.MemberID))
            .ForMember(dest => dest.EmployeeId, opt => opt.MapFrom(src => src.EmployeeId))
            .ForMember(dest => dest.ReturnEmployeeId, opt => opt.MapFrom(src => src.ReturnEmployeeID))
            .ForMember(dest => dest.Description, opt => opt.MapFrom(src => src.Description));
    }
}
