using AppFramework.Application;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Book;

namespace LibBook.ApplicationServices;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }

    public async Task<OperationResult> Create(BookDto dto)
    {
        OperationResult operationResult = new();
        if (_bookRepository.Exists(x => x.Title == dto.Title))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        Book book = new(dto.Title, dto.ISBN, dto.Code, dto.Description, dto.CategoryId, dto.AuthorId, dto.PublisherId, dto.TranslatorId);
        var result = await _bookRepository.CreateAsync(book);

        return operationResult.Succeeded();
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
                Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
                Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
                Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
                Category = book.Category.Name,
            }).ToList();

        return Task.FromResult(result);
    }

    public async Task<BookViewModel> GetById(int id)
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

    public async Task Delete(int id)
    {
        var result = await _bookRepository.GetByIdAsync(id);
        await _bookRepository.DeleteAsync(result);
    }

    public async Task<OperationResult> Update(BookViewModel dto)
    {
        OperationResult operationResult = new();
        var book = await _bookRepository.GetByIdAsync(dto.Id);
        if (book == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (_bookRepository.Exists(x => x.Title == dto.Title && x.Id != dto.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        book.Edit(dto.Title, dto.ISBN, dto.Code, dto.Description, dto.CategoryId, dto.AuthorId, dto.PublisherId, dto.TranslatorId);

        await _bookRepository.UpdateAsync(book);
        return operationResult.Succeeded();
    }

    public async Task<List<BookViewModel>> GetBooks()
    {
        return await _bookRepository.GetBooks();
    }
}