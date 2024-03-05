using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using MediatR;

namespace Library.Application.CQRS.Queries.Book;

public record  GetBookQuery(int id):IRequest<BookViewModel>
{
}
internal sealed class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookViewModel>
{
    private readonly IBookService _bookService;

    public GetBookQueryHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<BookViewModel> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookService.GetById(request.id);
        if (result != null) 
            return result;
        throw new Exception("Not Found");
    }
}
