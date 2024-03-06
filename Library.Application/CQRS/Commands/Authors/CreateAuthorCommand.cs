using Library.Domain.Entities.AuthorAgg;
using Library.Application.Interfaces;
using MediatR;
using Library.Application.DTOs.Authors;

namespace Library.Application.CQRS.Commands.Authors;

public record CreateAuthorCommand(AuthorDto dto) : IRequest<int>;
internal sealed class CreateAuthorCommandHandler(IAuthorRepository authorRepository) : IRequestHandler<CreateAuthorCommand, int>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;


    public async Task<int> Handle(CreateAuthorCommand request, CancellationToken cancellationToken)
    {
        var auther = new Author
        {
            Name = request.dto.Name,
            Description = request.dto.Description,
        };
        var result = await _authorRepository.CreateAsync(auther);

        return result.Id;
    }
}
