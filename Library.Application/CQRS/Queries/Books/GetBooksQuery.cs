using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Books;

public record GetAllBooksQuery : IRequest<List<BookViewModel>>
{
}
internal sealed class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookViewModel>>
{
    private readonly IBookRepository _bookRepository;

    public GetAllBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookViewModel>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        return await _bookRepository.GetBooks();
    }
}
