using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using MediatR;
using Microsoft.EntityFrameworkCore;

namespace Library.Application.CQRS.Queries.Books;

public record GetBookQuery(int Id) : IRequest<BookViewModel>
{
}
internal sealed class GetBookQueryHandler(IBookRepository bookRepository) : IRequestHandler<GetBookQuery, BookViewModel>
{
    private readonly IBookRepository _bookRepository = bookRepository;

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
            }).FirstOrDefaultAsync(b => b.Id == request.Id);

        return result ?? throw new Exception($"No book was found with this {request.Id}");
    }
}
