using Library.Application.Contracts;
using Library.Application.DTOs.BookCategory;
using MediatR;

namespace Library.Application.CQRS.Queries.BookCategory;

public record GetBookCategoryQuery(int id) : IRequest<BookCategoryDto>{}

public record GetBookCategoryQueryHandler : IRequestHandler<GetBookCategoryQuery, BookCategoryDto>
{
    private readonly IBookCategoryService _bookCategoryService;

    public GetBookCategoryQueryHandler(IBookCategoryService bookCategoryService)
    {
        _bookCategoryService = bookCategoryService;
    }

    public async Task<BookCategoryDto> Handle(GetBookCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryService.GetById(request.id);
        return result;
    }
}
