using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Authors;

public record GetAllAuthorsQuery : IRequest<List<AuthorDto>>;
internal sealed class GetAllAuthorsQueryHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAllAuthorsQuery, List<AuthorDto>>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;

    public async Task<List<AuthorDto>> Handle(GetAllAuthorsQuery request, CancellationToken cancellationToken)
    {
        var result = _authorRepository.GetAll()
            .Select(author => new AuthorDto
            {
                Id = author.Id,
                Name = author.Name,
                Description = author.Description,
            }).ToList();

        return await Task.FromResult(result);
    }
}
