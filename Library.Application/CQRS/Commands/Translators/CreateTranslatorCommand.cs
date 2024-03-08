using Library.Application.DTOs.Translators;
using Library.Application.Interfaces;
using Library.Domain.Entities.TranslatorAgg;
using MediatR;

namespace Library.Application.CQRS.Commands.Translators;

public record CreateTranslatorCommand(TranslatorDto dto) : IRequest<int>;
internal sealed class CreateTranslatorCommandHandler(ITranslatorRepository translatorRepository) : IRequestHandler<CreateTranslatorCommand, int>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<int> Handle(CreateTranslatorCommand request, CancellationToken cancellationToken)
    {
        var translator = new Translator
        {
            Name = request.dto.Name,
            Description = request.dto.Description,
        };
        var result = await _translatorRepository.CreateAsync(translator);
        return result.Id;
    }
}
