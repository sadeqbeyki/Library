using Library.Application.DTOs.Publishers;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Publishers;

public record GetAllPublishersQuery : IRequest<List<PublisherDto>>;
internal sealed class GetAllPublishersQueryHandler(IPublisherRepository publisherRepository) : IRequestHandler<GetAllPublishersQuery, List<PublisherDto>>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<List<PublisherDto>> Handle(GetAllPublishersQuery request, CancellationToken cancellationToken)
    {
        var result = _publisherRepository.GetAll()
            .Select(publisher => new PublisherDto
            {
                Id = publisher.Id,
                Name = publisher.Name,
                Description = publisher.Description,
            }).ToList();

        return result;
    }
}
