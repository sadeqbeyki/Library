using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategories;

public record DeleteBookCategoryCommand(int id) : IRequest<bool>;

internal sealed class DeleteBookCategoryCommandHandler(IBookCategoryRepository categoryRepository)
                                                        : IRequestHandler<DeleteBookCategoryCommand, bool>
{
    private readonly IBookCategoryRepository _categoryRepository = categoryRepository;

    public async Task<bool> Handle(DeleteBookCategoryCommand request, CancellationToken cancellationToken)
    {
        var category = await _categoryRepository.GetByIdAsync(request.id);
        var result = _categoryRepository.DeleteAsync(category);
        return result.IsCompletedSuccessfully;
    }
}