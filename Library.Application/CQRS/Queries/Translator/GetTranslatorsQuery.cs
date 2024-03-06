using Library.Application.Contracts;
using Library.Application.DTOs.Translator;
using MediatR;

namespace Library.Application.CQRS.Queries.Translator;

public record GetTranslatorsQuery : IRequest<List<TranslatorDto>>;
internal sealed class GetTranslatorsQueryHandler(ITranslatorService translatorService) : IRequestHandler<GetTranslatorsQuery, List<TranslatorDto>>
{
    private readonly ITranslatorService _translatorService = translatorService;

    public async Task<List<TranslatorDto>> Handle(GetTranslatorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _translatorService.GetAll();
        return result;
    }
}
