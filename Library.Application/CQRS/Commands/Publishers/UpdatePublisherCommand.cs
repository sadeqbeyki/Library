using Library.Application.DTOs.Publishers;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Publishers;

public record UpdatePublisherCommand(PublisherDto dto) : IRequest<int>;
internal sealed class UpdatePublisherCommandHandler(IPublisherRepository publisherRepository) : IRequestHandler<UpdatePublisherCommand, int>
{
    private readonly IPublisherRepository _publisherRepository = publisherRepository;

    public async Task<int> Handle(UpdatePublisherCommand request, CancellationToken cancellationToken)
    {
        var publisher = await _publisherRepository.GetByIdAsync(request.dto.id);
        if (publisher != null)
        {
            publisher.Name = request.dto.Name;
            publisher.Description = request.dto.Description;

            await _publisherRepository.UpdateAsync(publisher);
            return request.dto.Id;
        }
        return 0;
    }
}
