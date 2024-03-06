using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.CQRS.Queries.Book;

public record GetBooksQuery:IRequest<List<BookViewModel>>
{
}
internal sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookViewModel>>
{
    private readonly IBookService _bookService;

    public GetBooksQueryHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<List<BookViewModel>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookService.GetBooks();
        if (result != null) 
            return result;
        throw new Exception("Not Found");
    }
}
