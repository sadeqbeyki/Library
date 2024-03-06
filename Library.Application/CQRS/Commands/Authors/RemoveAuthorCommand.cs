using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Authors;

public record RemoveAuthorCommand(int id) : IRequest;
internal sealed class RemoveAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<RemoveAuthorCommand>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;

    public async Task Handle(RemoveAuthorCommand request, CancellationToken cancellationToken)
    {
        var result = await _authorRepository.GetByIdAsync(request.id);
        await _authorRepository.DeleteAsync(result);
    }
}
