using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Publishers;

public record DeletePublisherCommand(int id) : IRequest<int>;
internal sealed class DeletePublisherCommandHandler(IPublisherRepository publisherRepository) : IRequestHandler<DeletePublisherCommand, int>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<int> Handle(DeletePublisherCommand request, CancellationToken cancellationToken)
    {
        var result = await _publisherRepository.GetByIdAsync(request.id);
        await _publisherRepository.DeleteAsync(result);
        return request.id;
    }
}
