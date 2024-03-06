using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.CQRS.Queries.Book;

public record GetAllBooksQuery : IRequest<List<BookViewModel>>
{
}
internal sealed class GetAllBooksQueryHandler : IRequestHandler<GetAllBooksQuery, List<BookViewModel>>
{
    private readonly IBookService _bookService;

    public GetAllBooksQueryHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<List<BookViewModel>> Handle(GetAllBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookService.GetAll();
        if (result != null) 
            return result;
        throw new Exception("Not Found");
    }
}
