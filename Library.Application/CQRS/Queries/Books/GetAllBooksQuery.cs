using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Books;

public record GetBooksQuery : IRequest<List<BookViewModel>>
{
}
internal sealed class GetBooksQueryHandler : IRequestHandler<GetBooksQuery, List<BookViewModel>>
{
    private readonly IBookRepository _bookRepository;

    public GetBooksQueryHandler(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<List<BookViewModel>> Handle(GetBooksQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookRepository.GetAll()
            .Select(book => new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Code = book.Code,
                Description = book.Description,
                Category = book.Category.Name,
            }).ToListAsync();

        return result;
    }
}
