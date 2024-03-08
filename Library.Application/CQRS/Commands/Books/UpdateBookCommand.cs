using AppFramework.Application;
using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.AspNetCore.Http;

namespace Library.Application.CQRS.Commands.Books;

public record UpdateBookCommand(BookViewModel Dto, IFormFile image) : IRequest<OperationResult>
{
}

internal sealed class UpdateBookCommandHandler : IRequestHandler<UpdateBookCommand, OperationResult>
{
    private readonly IBookService _bookService;
    private readonly IBookRepository _bookRepository;

    public UpdateBookCommandHandler(IBookService bookService, IBookRepository bookRepository)
    {
        _bookService = bookService;
        _bookRepository = bookRepository;
    }

    public async Task<OperationResult> Handle(UpdateBookCommand request, CancellationToken cancellationToken)
    {
        OperationResult operationResult = new();
        var book = await _bookRepository.GetByIdAsync(request.Dto.Id);

        if (book == null) return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (_bookRepository.Exists(x => x.Title == request.Dto.Title && x.Id != request.Dto.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        byte[]? picture = await _bookService.ConvertImageToByte(request.image);

        book.Edit(request.Dto.Title, request.Dto.ISBN, request.Dto.Code, request.Dto.Description, request.Dto.CategoryId, picture);

        book.BookAuthors.Clear();
        book.BookPublishers.Clear();
        book.BookTranslators.Clear();

        await _bookService.AddAuthors(request.Dto, book);
        await _bookService.AddPublishers(request.Dto, book);
        await _bookService.AddTranslators(request.Dto, book);

        var result = _bookRepository.UpdateAsync(book);
        return result == null
                            ? operationResult.Failed(ApplicationMessages.ProblemFound)
                            : operationResult.Succeeded();
    }
}
