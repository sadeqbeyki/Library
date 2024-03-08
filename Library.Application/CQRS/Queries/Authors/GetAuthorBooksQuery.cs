using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Authors;

public record GetAuthorBooksQuery(int authorId):IRequest<List<BookViewModel>>
{
}
internal sealed class GetAuthorBooksQueryHandler(IAuthorRepository authorRepository) : IRequestHandler<GetAuthorBooksQuery, List<BookViewModel>>
{
    private readonly IAuthorRepository _authorRepository = authorRepository;
    public async Task<List<BookViewModel>> Handle(GetAuthorBooksQuery request, CancellationToken cancellationToken)
    {
        var books = await _authorRepository.GetAuthorBooks(request.authorId);
        var result = books.Select(b => new BookViewModel
        {
            Title = b.Title,
            ISBN = b.ISBN,
            Code = b.Code,
            Description = b.Description,
            CategoryId = b.CategoryId,
        }).ToList();
        return result;
    }
}
