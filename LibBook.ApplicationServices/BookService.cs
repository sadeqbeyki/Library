using AppFramework.Application;
using LibBook.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BookCategoryAgg;
using LibBook.Domain.PublisherAgg;
using LibBook.Domain.TranslatorAgg;
using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using Microsoft.EntityFrameworkCore;

namespace LibBook.ApplicationServices;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ITranslatorRepository _translatorRepository;
    private readonly IPublisherRepository _publisherRepository;
    private readonly IBookCategoryRepository _bookCategoryRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository,
        ITranslatorRepository translatorRepository, IPublisherRepository publisherRepository, IBookCategoryRepository bookCategoryRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _translatorRepository = translatorRepository;
        _publisherRepository = publisherRepository;
        _bookCategoryRepository = bookCategoryRepository;
    }
    #region Create
    public async Task<OperationResult> Create(BookDto model)
    {
        OperationResult operationResult = new();
        if (_bookRepository.Exists(x => x.Title == model.Title))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        Book book = new(model.Title, model.ISBN, model.Code, model.Description, model.CategoryId, model.Picture);

        await AddAuthors(model, book);
        await AddPublishers(model, book);
        await AddTranslators(model, book);

        var result = await _bookRepository.CreateAsync(book);
        return result == null 
            ? operationResult.Failed(ApplicationMessages.ProblemFound) 
            : operationResult.Succeeded();
    }
    private async Task AddAuthors(BookDto model, Book book)
    {
        if (model.Authors != null && model.Authors.Any())
        {
            foreach (var authorName in model.Authors)
            {
                var author = await _authorRepository.GetByName(authorName);
                if (author != null)
                {
                    var bookAuthor = new BookAuthor
                    {
                        AuthorBookId = book.Id,
                        Book = book,
                        AuthorId = author.Id,
                        Author = author,
                    };
                    book.BookAuthors.Add(bookAuthor);
                }
                //else
            }
        }
    }
    private async Task AddPublishers(BookDto model, Book book)
    {
        if (model.Publishers != null && model.Publishers.Any())
        {
            foreach (var publisherName in model.Publishers)
            {
                var publisher = await _publisherRepository.GetByName(publisherName);
                if (publisher != null)
                {
                    var bookPublisher = new BookPublisher
                    {
                        PublisherBookId = book.Id,
                        Book = book,
                        PublisherId = publisher.Id,
                        Publisher = publisher
                    };
                    book.BookPublishers.Add(bookPublisher);
                }
                //else
            }
        }
    }
    private async Task AddTranslators(BookDto model, Book book)
    {
        if (model.Translators != null && model.Translators.Any())
        {
            foreach (var translatorName in model.Translators)
            {
                var translator = await _translatorRepository.GetByName(translatorName);
                if (translator != null)
                {
                    var bookTranslator = new BookTranslator
                    {
                        TranslatorBookId = book.Id,
                        Book = book,
                        TranslatorId = translator.Id,
                        Translator = translator
                    };
                    book.BookTranslators.Add(bookTranslator);
                }
                //else
            }
        }
    }

    #endregion

    #region Read
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
                Category = book.Category.Name,
            }).ToList();

        return Task.FromResult(result);
    }

    //select box in inventory
    public async Task<List<BookViewModel>> GetBooks()
    {
        return await _bookRepository.GetBooks();
    }

    public List<BookViewModel> Search(BookSearchModel searchModel)
    {
        return _bookRepository.Search(searchModel);
    }

    public async Task<BookViewModel> GetById(int id)
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
            }).FirstOrDefaultAsync(b => b.Id == id);

        return result;
    }
    #endregion

    #region Update

    public async Task<OperationResult> Update(BookViewModel model)
    {
        OperationResult operationResult = new();

        // 1. Check if the book exists
        var book = await _bookRepository.GetByIdAsync(model.Id);
        if (book == null)
        {
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        }

        // 1.1. Check if duplicate found
        if (_bookRepository.Exists(x => x.Title == model.Title && x.Id != model.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        // 2. Update the book properties
        book.Edit(model.Title, model.ISBN, model.Code, model.Description, model.CategoryId, model.Picture);

        // 3. Clear the existing relationships with authors, publishers, and translators
        book.BookAuthors.Clear();
        book.BookPublishers.Clear();
        book.BookTranslators.Clear();

        // 4. Add Authors
        if (model.Authors != null && model.Authors.Any())
        {
            foreach (var authorName in model.Authors)
            {
                var author = await _authorRepository.GetByName(authorName);
                if (author != null)
                {
                    var bookAuthor = new BookAuthor
                    {
                        AuthorBookId = book.Id,
                        Book = book,
                        AuthorId = author.Id,
                        Author = author,
                    };
                    book.BookAuthors.Add(bookAuthor);
                }
            }
        }

        // 5. Add Publishers
        if (model.Publishers != null && model.Publishers.Any())
        {
            foreach (var publisherName in model.Publishers)
            {
                var publisher = await _publisherRepository.GetByName(publisherName);
                if (publisher != null)
                {
                    var bookPublisher = new BookPublisher
                    {
                        PublisherBookId = book.Id,
                        Book = book,
                        PublisherId = publisher.Id,
                        Publisher = publisher
                    };
                    book.BookPublishers.Add(bookPublisher);
                }
            }
        }

        // 6. Add Translators
        if (model.Translators != null && model.Translators.Any())
        {
            foreach (var translatorName in model.Translators)
            {
                var translator = await _translatorRepository.GetByName(translatorName);
                if (translator != null)
                {
                    var bookTranslator = new BookTranslator
                    {
                        TranslatorBookId = book.Id,
                        Book = book,
                        TranslatorId = translator.Id,
                        Translator = translator
                    };
                    book.BookTranslators.Add(bookTranslator);
                }
            }
        }

        // 7. Update the book in the database
        var result = _bookRepository.UpdateAsync(book);
        //tryyyyyyyyyyy cashhhhhhhhhhhhhhh

        if (result == null)
        {
            return operationResult.Failed(ApplicationMessages.ProblemFound);
        }

        return operationResult.Succeeded();
    }
    #endregion

    #region Delete
    public async Task Delete(int id)
    {
        var result = await _bookRepository.GetByIdAsync(id);
        await _bookRepository.DeleteAsync(result);
    }
    #endregion

}