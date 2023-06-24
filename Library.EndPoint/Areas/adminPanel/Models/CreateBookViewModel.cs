﻿using LMS.Contracts.Author;
using LMS.Contracts.BookCategoryContract;
using LMS.Contracts.Publisher;
using LMS.Contracts.Translator;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class CreateBookViewModel
{
    public Guid PublisherId { get; set; }
    public Guid AuthorId { get; set; }
    public Guid TranslatorId { get; set; }
    public Guid CategoryId { get; set; }

    public string Title { get; set; }
    public string ISBN { get; set; }
    public string Code { get; set; }
    public string Description { get; set; }

    public List<BookCategoryDto> BookCategories { get; set; } = new List<BookCategoryDto>();
    public List<AuthorDto> Authors { get; set; } = new List<AuthorDto>();
    public List<PublisherDto> Publishers { get; set; } = new List<PublisherDto>();
    public List<TranslatorDto> Translators { get; set; } = new List<TranslatorDto>();

}
