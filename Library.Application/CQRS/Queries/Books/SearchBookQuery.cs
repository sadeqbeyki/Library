using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;

namespace Library.Application.CQRS.Queries.Books;

public record SearchBookQuery(BookSearchModel dto) : IRequest<List<BookViewModel>>
{
}
internal sealed class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, List<BookViewModel>>
{
    private readonly IBookRepository _bookRepository;

    public SearchBookQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookViewModel>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
    {
        return _bookRepository.Search(request.dto);

    }
}
