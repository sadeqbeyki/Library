using Library.Application.Contracts;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategory;

public record DeleteBookCategoryCommand(int id) : IRequest<bool>;

internal sealed class DeleteBookCategoryCommandHandler(IBookCategoryService bookCategoryService)
                                                        : IRequestHandler<DeleteBookCategoryCommand,bool>
{
    private readonly IBookCategoryService _bookCategoryService = bookCategoryService;

    public async Task<bool> Handle(DeleteBookCategoryCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookCategoryService.Delete(request.id);
        return result;
    }
}