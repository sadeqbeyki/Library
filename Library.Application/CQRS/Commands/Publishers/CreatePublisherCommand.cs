using Library.Application.DTOs.Publishers;
using Library.Application.Interfaces;
using Library.Domain.Entities.PublisherAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.Publishers;

public record CreatePublisherCommand(PublisherDto dto) : IRequest<int>;
internal sealed class CreatePublisherCommandHandler(IPublisherRepository publisherRepository) : IRequestHandler<CreatePublisherCommand, int>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<int> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = new Publisher
        {
            Name = request.dto.Name,
            Description = request.dto.Description,
        };
        var result = await _publisherRepository.CreateAsync(publisher);

        return result.Id;
    }
}
