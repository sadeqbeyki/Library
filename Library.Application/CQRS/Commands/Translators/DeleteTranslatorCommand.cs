using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Translators;

public record DeleteTranslatorCommand(int id) : IRequest<int>;
internal sealed class DeleteTranslatorCommandHandler(ITranslatorRepository translatorRepository) : IRequestHandler<DeleteTranslatorCommand, int>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<int> Handle(DeleteTranslatorCommand request, CancellationToken cancellationToken)
    {
        var result = await _translatorRepository.GetByIdAsync(request.id);
        await _translatorRepository.DeleteAsync(result);
        return request.id;
    }
}
