﻿using AutoMapper;
using LMS.Contracts.BookCategoryContract;
using LMS.Domain.BookCategoryAgg;

namespace LMS.Domain;

public class MappingProfile : Profile
{
    public MappingProfile()
    {
        CreateMap<BookCategoryDto, BookCategory>().ReverseMap();
        //CreateMap<UpdateUserDto, User>().ReverseMap();
        //CreateMap<RoleDto, Role>().ReverseMap();
    }
}