using Library.Application.DTOs.Translators;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Translators;

public record UpdateTranslatorCommand(int id, TranslatorDto dto) : IRequest<int>;
internal sealed class UpdateTranslatorCommandHandler(ITranslatorRepository translatorRepository) 
                                                        : IRequestHandler<UpdateTranslatorCommand, int>
{
    private readonly ITranslatorRepository _translatorRepository = translatorRepository;

    public async Task<int> Handle(UpdateTranslatorCommand request, CancellationToken cancellationToken)
    {
        var translator = await _translatorRepository.GetByIdAsync(request.id);
        if (translator == null)
            throw new KeyNotFoundException("i couldn't find a translator with this id!");

        translator.Name = request.dto.Name;
        translator.Description = request.dto.Description;

        await _translatorRepository.UpdateAsync(translator);
        return request.id;
    }
}
