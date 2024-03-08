using Library.Application.DTOs.BookCategories;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.BookCategories;

public record GetBookCategoryQuery(int id) : IRequest<BookCategoryDto> { }

public record GetBookCategoryQueryHandler(IBookCategoryRepository bookCategoryRepository) 
                                            : IRequestHandler<GetBookCategoryQuery, BookCategoryDto>
{
    private readonly IBookCategoryRepository _bookCategoryRepository = bookCategoryRepository;
    public async Task<BookCategoryDto> Handle(GetBookCategoryQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryRepository.GetByIdAsync(request.id);
        BookCategoryDto dto = new()
        {
            Id = request.id,
            Name = result.Name,
            Description = result.Description,
        };
        return dto;
    }
}
