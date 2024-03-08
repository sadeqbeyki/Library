using Library.Application.DTOs.Translators;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Translators;

public record GetTranslatorQuery(int id) : IRequest<TranslatorDto>;
internal sealed class GetTranslatorQueryHandler(ITranslatorRepository translatorRepository) : IRequestHandler<GetTranslatorQuery, TranslatorDto>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<TranslatorDto> Handle(GetTranslatorQuery request, CancellationToken cancellationToken)
    {
        var result = await _translatorRepository.GetByIdAsync(request.id);
        TranslatorDto dto = new()
        {
            Id = request.id,
            Name = result.Name,
            Description = result.Description,
        };
        return dto;
    }
}
