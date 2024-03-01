using Library.Application.Contracts;
using Library.Application.DTOs.Author;
using Library.Application.DTOs.Publisher;
using MediatR;

namespace Library.Application.CQRS.Commands.Author;

public record GetPublishersQuery : IRequest<List<PublisherDto>>;
internal sealed class GetPublishersQueryHandler(IPublisherService publisherService) : IRequestHandler<GetPublishersQuery, List<PublisherDto>>
{
    private readonly IPublisherService _publisherService = publisherService;

    public async Task<List<PublisherDto>> Handle(GetPublishersQuery request, CancellationToken cancellationToken)
    {
        var result = await _publisherService.GetAll();
        return result;
    }
}
