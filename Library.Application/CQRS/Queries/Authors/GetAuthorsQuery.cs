using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Authors;

public record GetAuthorsQuery : IRequest<List<AuthorDto>>;
internal sealed class GetAuthorsQueryHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorsQuery, List<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;

    public Task<List<AuthorDto>> Handle(GetAuthorsQuery request, CancellationToken cancellationToken)
    {
        var result = _authorRepository.GetAuthors();
        return result;
    }
}
