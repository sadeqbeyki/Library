using Library.Application.DTOs.Authors;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Commands.Authors;

public record GetAuthorByIdQuery(int id) : IRequest<AuthorDto>;
internal sealed class GetAuthorByIdQueryHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorByIdQuery, AuthorDto>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;

    public async Task<AuthorDto> Handle(GetAuthorByIdQuery request, CancellationToken cancellationToken)
    {
        var result = await _authorRepository.GetByIdAsync(request.id);
        AuthorDto dto = new()
        {
            Id = request.id,
            Name = result.Name,
            Description = result.Description,
        };
        return dto;
    }
}
