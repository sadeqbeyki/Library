using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Books;

public record  GetBookQuery(int id):IRequest<BookViewModel>
{
}
internal sealed class GetBookQueryHandler : IRequestHandler<GetBookQuery, BookViewModel>
{
    private readonly IBookService _bookService;
    private readonly IBookRepository _bookRepository;

    public GetBookQueryHandler(IBookService bookService)
    {
        _bookService = bookService;
    }

    public async Task<BookViewModel> Handle(GetBookQuery request, CancellationToken cancellationToken)
    {
        var result = await _bookRepository.GetAll()
            .Select(book => new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Code = book.Code,
                Description = book.Description,
                CategoryId = book.CategoryId,
                Picture = book.Picture,

                Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
                Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
                Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
                Category = book.Category.Name,
            }).FirstOrDefaultAsync(b => b.Id == request.id);

        return result ?? throw new Exception($"No book was found with this {request.id}");
    }
}
