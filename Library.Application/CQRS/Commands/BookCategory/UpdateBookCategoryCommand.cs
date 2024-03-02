using Library.Application.Contracts;
using Library.Application.DTOs.BookCategory;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategory;

public record UpdateBookCategoryCommand(int id, BookCategoryDto dto) : IRequest<bool>;

internal sealed class UpdateBookCategoryCommandHandler(IBookCategoryService bookCategoryService)
                                                        : IRequestHandler<UpdateBookCategoryCommand, bool>
{
    private readonly IBookCategoryService _bookCategoryService = bookCategoryService;

    public async Task<bool> Handle(UpdateBookCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryService.Update(request.id, request.dto);
        return result.IsSucceeded;
    }
}