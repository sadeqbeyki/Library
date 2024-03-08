using Library.Application.DTOs.BookCategories;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.BookCategories;

public record GetBookCategoriesQuery : IRequest<List<BookCategoryDto>> { }

public record GetBookCategoriesQueryHandler : IRequestHandler<GetBookCategoriesQuery, List<BookCategoryDto>>
{
    private readonly IBookCategoryRepository _bookCategoryRepository;

    public GetBookCategoriesQueryHandler(IBookCategoryRepository bookCategoryRepository)
    {
        _bookCategoryRepository = bookCategoryRepository;
    }

    public async Task<List<BookCategoryDto>> Handle(GetBookCategoriesQuery request, CancellationToken cancellationToken)
    {
        return await _bookCategoryRepository.GetCategories();
    }
}
