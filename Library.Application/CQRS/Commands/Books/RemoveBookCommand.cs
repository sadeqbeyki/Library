using Library.Application.Contracts;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Books;

public record RemoveBookCommand(int id) : IRequest
{
}
internal sealed class RemoveBookCommandHandler : IRequestHandler<RemoveBookCommand>
{
    private readonly IBookRepository _bookRepository;

    public RemoveBookCommandHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task Handle(RemoveBookCommand request, CancellationToken cancellationToken)
    {
        var result = await _bookRepository.GetByIdAsync(request.id);
        await _bookRepository.DeleteAsync(result);
    }
}
