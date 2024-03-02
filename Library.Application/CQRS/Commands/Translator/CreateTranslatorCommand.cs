using Library.Application.Contracts;
using Library.Application.DTOs.Translator;
using MediatR;

namespace Library.Application.CQRS.Commands.Author;

public record CreateTranslatorCommand(TranslatorDto dto) : IRequest<bool>;
internal sealed class CreateTranslatorCommandHandler(ITranslatorService translatorService) : IRequestHandler<CreateTranslatorCommand, bool>
{
    private readonly ITranslatorService _translatorService = translatorService;

    public async Task<bool> Handle(CreateTranslatorCommand request, CancellationToken cancellationToken)
    {
        var result = await _translatorService.Create(request.dto);
        return result.Id > 0;
    }
}
