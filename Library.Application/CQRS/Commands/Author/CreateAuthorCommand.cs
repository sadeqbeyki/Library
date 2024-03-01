using Library.Application.Contracts;
using Library.Application.DTOs.Author;
using MediatR;

namespace Library.Application.CQRS.Commands.Author;

public record CreateAuthorCommand(AuthorDto dto) : IRequest<bool>;
internal sealed class CreateAuthorCommandHandler(IAuthorService authorService) : IRequestHandler<CreateAuthorCommand, bool>
{
    private readonly IAuthorService _authorService = authorService;

    public async Task<bool> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var result = await _authorService.Create(request.dto);
        return result.Id > 0;
    }
}
