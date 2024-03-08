using Library.Application.DTOs.Translators;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Translators;

public record GetAllTranslatorsQuery : IRequest<List<TranslatorDto>>;
internal sealed class GetAllTranslatorsQueryHandler(ITranslatorRepository translatorRepository) : IRequestHandler<GetAllTranslatorsQuery, List<TranslatorDto>>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<List<TranslatorDto>> Handle(GetAllTranslatorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _translatorRepository.GetAll()
            .Select(translator => new TranslatorDto
            {
                Id = translator.Id,
                Name = translator.Name,
                Description = translator.Description,
            }).ToListAsync();

        return result;
    }
}
