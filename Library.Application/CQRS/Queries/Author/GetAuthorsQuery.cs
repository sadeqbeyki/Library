using Library.Application.Contracts;
using Library.Application.DTOs.Author;
using MediatR;

namespace Library.Application.CQRS.Commands.Author;

public record GetAuthorsQuery : IRequest<List<AuthorDto>>;
internal sealed class GetAuthorsQueryHandler(IAuthorService authorService) : IRequestHandler<GetAuthorsQuery, List<AuthorDto>>
{
    private readonly IAuthorService _authorService = authorService;

    public async Task<List<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var result = await _authorService.GetAll();
        return result;
    }
}
