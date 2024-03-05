using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.CQRS.Commands.Book;

public record CreateBookCommand(CreateBookModel Dto) : IRequest<bool>
{
}

internal sealed class CreateBookCommandHandler : IRequestHandler<CreateBookCommand, bool>
{
    private readonly IBookService _bookService;

    public CreateBookCommandHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<bool> Handle(CreateBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookService.Create(request.Dto);
        return result.IsSucceeded ? result.IsSucceeded : false;
    }
}

