using Library.Application.Contracts;
using MediatR;

namespace Library.Application.CQRS.Commands.Book;

public record RemoveBookCommand(int id) : IRequest
{
}
internal sealed class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand>
{
    private readonly IBookService _bookService;

    public RemoveBookCommandHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        await _bookService.Delete(request.id);
    }
}
