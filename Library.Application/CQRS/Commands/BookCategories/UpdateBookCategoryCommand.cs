using AppFramework.Application;
using Library.Application.DTOs.BookCategories;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategories;

public record UpdateBookCategoryCommand(int id, BookCategoryDto dto) : IRequest<OperationResult>;

internal sealed class UpdateBookCategoryCommandHandler(IBookCategoryRepository categoryRepository)
                                                        : IRequestHandler<UpdateBookCategoryCommand, OperationResult>
{
    private readonly IBookCategoryRepository _categoryRepository = categoryRepository;


    public async Task<OperationResult> Handle(UpdateBookCategoryCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();
        var bookCategory = await _categoryRepository.GetByIdAsync(request.id);
        if (bookCategory == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (_categoryRepository.Exists(x => x.Name == request.dto.Name && x.Id != request.dto.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        bookCategory.Edit(request.dto.Name, request.dto.Description);
        await _categoryRepository.UpdateAsync(bookCategory);
        return operationResult.Succeeded();
    }
}