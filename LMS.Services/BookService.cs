using LMS.Contracts.Book;
using LMS.Domain.BookAgg;


namespace LMS.Services;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<BookDto> Create(BookDto dto)
    {
        var book = new Book
        {
            Title = dto.Title,
            ISBN = dto.ISBN,
            Code = dto.Code,
            Description = dto.Description,
            AuthorId = dto.AuthorId,
            PublisherId = dto.PublisherId,
            TranslatorId = dto.TranslatorId,
            CategoryId = dto.CategoryId,

        };
        var addBook = await _bookRepository.AddAsync(book);

        var result = new BookDto
        {
            Title = addBook.Title,
            ISBN = addBook.ISBN,
            Code = addBook.Code,
            Description = addBook.Description,
            AuthorId = addBook.AuthorId,
            PublisherId = addBook.PublisherId,
            TranslatorId = addBook.TranslatorId,
            CategoryId = addBook.CategoryId,
        };

        return result;
    }

    public Task<List<BookViewModel>> GetAll()
    {
        var result = _bookRepository.GetAll()
            .Select(book => new BookViewModel
            {
                Id = book.Id,
                Title = book.Title,
                ISBN = book.ISBN,
                Code = book.Code,
                Description = book.Description,
                AuthorId = book.AuthorId,
                PublisherId = book.PublisherId,
                TranslatorId = book.TranslatorId,
                CategoryId = book.CategoryId,
            }).ToList();

        return Task.FromResult(result);
    }

    public async Task<BookViewModel> GetById(Guid id)
    {
        var result = await _bookRepository.GetByIdAsync(id);
        BookViewModel dto = new()
        {
            Id = id,
            Title = result.Title,
            ISBN = result.ISBN,
            Code = result.Code,
            Description = result.Description,
            AuthorId = result.AuthorId,
            PublisherId = result.PublisherId,
            TranslatorId = result.TranslatorId,
            CategoryId = result.CategoryId,
        };
        return dto;
    }

    public async Task Delete(Guid id)
    {
        var result = await _bookRepository.GetByIdAsync(id);
        await _bookRepository.DeleteAsync(result);
    }

    public async Task<BookDto> Update(Guid id, BookDto entity)
    {
        var existingBook = await _bookRepository.GetByIdAsync(id);
        if (existingBook == null)
            return null;

        existingBook.Title = entity.Title;
        existingBook.Description = entity.Description;
        existingBook.ISBN = entity.ISBN; 
        existingBook.Code = entity.Code;
        existingBook.AuthorId = entity.AuthorId;
        existingBook.PublisherId = entity.PublisherId;
        existingBook.TranslatorId = entity.TranslatorId;
        existingBook.CategoryId = entity.CategoryId;

        await _bookRepository.UpdateAsync(existingBook);
        return entity;
    }

    public async Task<List<BookViewModel>> GetBooks()
    {
        return await _bookRepository.GetBooks();
    }
}