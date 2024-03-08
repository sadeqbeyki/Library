using Library.Application.DTOs.Publishers;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Publishers;

public record GetPublishersQuery : IRequest<List<PublisherDto>>;
internal sealed class GetPublishersQueryHandler(IPublisherRepository publisherRepository) : IRequestHandler<GetPublishersQuery, List<PublisherDto>>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<List<PublisherDto>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
    {
        return await _publisherRepository.GetPublishers();

    }
}
