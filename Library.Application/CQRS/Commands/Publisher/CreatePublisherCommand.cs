using Library.Application.Contracts;
using Library.Application.DTOs.Publisher;
using MediatR;

namespace Library.Application.CQRS.Commands.Publisher;

public record CreatePublisherCommand(PublisherDto dto) : IRequest<bool>;
internal sealed class CreatePublisherCommandHandler(IPublisherService publisherService) : IRequestHandler<CreatePublisherCommand, bool>
{
    private readonly IPublisherService _publisherService = publisherService;

    public async Task<bool> Handle(CreatePublisherCommand request, CancellationToken cancellationToken)
    {
        var result = await _publisherService.Create(request.dto);
        return result.Id > 0;
    }
}
