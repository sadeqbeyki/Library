using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.CQRS.Queries.Book;

public record SearchBookQuery(BookSearchModel dto) : IRequest<List<BookViewModel>>
{
}
internal sealed class SearchBookQueryHandler : IRequestHandler<SearchBookQuery, List<BookViewModel>>
{
    private readonly IBookService _bookService;

    public SearchBookQueryHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public Task<List<BookViewModel>> Handle(SearchBookQuery request, CancellationToken cancellationToken)
    {
        var result = _bookService.Search(request.dto);
        if (result != null)
            return Task.FromResult(result);
        throw new Exception("not found"); 
    }
}
