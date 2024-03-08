using Library.Application.DTOs.Translators;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Translators;

public record GetTranslatorsQuery : IRequest<List<TranslatorDto>>;
internal sealed class GetTranslatorsQueryHandler(ITranslatorRepository translatorRepository) 
                                                    : IRequestHandler<GetTranslatorsQuery, List<TranslatorDto>>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<List<TranslatorDto>> Handle(GetTranslatorsQuery request, CancellationToken cancellationToken)
    {
        return await _translatorRepository.GetTranslators();
    }
}
