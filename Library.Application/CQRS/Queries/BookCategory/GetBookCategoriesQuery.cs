using Library.Application.Contracts;
using Library.Application.DTOs.BookCategory;
using MediatR;

namespace Library.Application.CQRS.Queries.BookCategory;

public record GetBookCategoriesQuery : IRequest<List<BookCategoryDto>>{}

public record GetBookCategoriesQueryHandler : IRequestHandler<GetBookCategoriesQuery, List<BookCategoryDto>>
{
    private readonly IBookCategoryService _bookCategoryService;

    public GetBookCategoriesQueryHandler(IBookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    public async Task<List<BookCategoryDto>> Handle(GetBookCategoriesQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryService.GetCategories();
        return result;
    }
}
