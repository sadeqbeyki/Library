using MediatR;
using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;

namespace Library.Application.CQRS.Commands.Authors;

public record UpdateAuthorCommand(AuthorDto Dto) : IRequest<int>;

internal sealed class UpdateAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<UpdateAuthorCommand, int>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;

    public async Task<int> Handle(UpdateAuthorCommand request, CancellationToken cancellationToken)
    {
        var author = await _authorRepository.GetByIdAsync(request.Dto.Id);
        if (author != null)
        author.Name = request.Dto.Name;
        author.Description = request.Dto.Description;

        await _authorRepository.UpdateAsync(author);
        return author.Id;
    }
}