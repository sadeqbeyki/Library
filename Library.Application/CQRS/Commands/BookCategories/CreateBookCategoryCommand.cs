using AppFramework.Application;
using Library.Application.DTOs.BookCategories;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookCategoryAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.BookCategories;

public record CreateBookCategoryCommand(BookCategoryDto dto) : IRequest<OperationResult>;
internal sealed class CreateBookCategoryCommandHandler(IBookCategoryRepository categoryRepository)
                                                        : IRequestHandler<CreateBookCategoryCommand, OperationResult>
{
    private readonly IBookCategoryRepository _categoryRepository = categoryRepository;

    public async Task<OperationResult> Handle(CreateBookCategoryCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();
        if (_categoryRepository.Exists(x => x.Name == request.dto.Name))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        BookCategory bookCategory = new(request.dto.Name, request.dto.Description);
        await _categoryRepository.CreateAsync(bookCategory);
        return operationResult.Succeeded();
    }
}

