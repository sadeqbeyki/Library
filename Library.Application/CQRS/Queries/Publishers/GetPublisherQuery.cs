using Library.Application.DTOs.Publishers;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Publishers;

public record GetPublisherQuery(int id) : IRequest<PublisherDto>;
internal sealed class GetPublisherQueryHandler(IPublisherRepository publisherRepository) : IRequestHandler<GetPublisherQuery, PublisherDto>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<PublisherDto> Handle(GetPublisherQuery request, CancellationToken cancellationToken)
    {
        var result = await _publisherRepository.GetByIdAsync(request.id);
        PublisherDto dto = new()
        {
            Id = request.id,
            Name = result.Name,
            Description = result.Description,
        };
        return dto;
    }
}
