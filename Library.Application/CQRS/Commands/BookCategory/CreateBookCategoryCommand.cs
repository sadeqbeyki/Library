using Library.Application.Contracts;
using Library.Application.DTOs.BookCategory;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategory;

public record CreateBookCategoryCommand(BookCategoryDto dto) : IRequest<bool>;
internal sealed class CreateBookCategoryCommandHandler(IBookCategoryService bookCategoryService) 
                                                        : IRequestHandler<CreateBookCategoryCommand, bool>
{
    private readonly IBookCategoryService _bookCategoryService = bookCategoryService;

    public async Task<bool> Handle(CreateBookCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryService.Create(request.dto);
        return result.IsSucceeded;
    }
}
