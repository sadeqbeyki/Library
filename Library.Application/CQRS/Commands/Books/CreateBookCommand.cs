using AppFramework.Application;
using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.Books;

public record CreateBookCommand(CreateBookModel Dto) : IRequest<OperationResult>
{
}

internal sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, OperationResult>
{
    private readonly IBookRepository _bookRepository;
    private readonly IBookService _bookService;

    public CreateBookCommandHandler(IBookRepository bookRepository, IBookService bookService)
    {
        _bookRepository = bookRepository;
        _bookService = bookService;
    }

    public async Task<OperationResult> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();
        if ( _bookRepository.Exists(x => x.Title == request.Dto.Title))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        byte[]? picture = await _bookService.ConvertImageToByte(request.Dto.Image);
        Book book = new(request.Dto.Title, request.Dto.ISBN, request.Dto.Code, request.Dto.Description, request.Dto.CategoryId, picture);

        await _bookService.AddAuthors(request.Dto, book);
        await _bookService.AddPublishers(request.Dto, book);
        await _bookService.AddTranslators(request.Dto, book);

        var result = await _bookRepository.CreateAsync(book);
        return result == null
            ? operationResult.Failed(ApplicationMessages.ProblemFound)
            : operationResult.Succeeded();
    }
}

