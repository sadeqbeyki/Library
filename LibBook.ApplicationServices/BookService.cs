using AppFramework.Application;
using LibBook.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.Domain.PublisherAgg;
using LibBook.Domain.TranslatorAgg;
using LibBook.DomainContracts.Book;
using Microsoft.EntityFrameworkCore;

namespace LibBook.ApplicationServices;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;
    private readonly ITranslatorRepository _translatorRepository;
    private readonly IPublisherRepository _publisherRepository;

    public BookService(IBookRepository bookRepository, IAuthorRepository authorRepository,
        ITranslatorRepository translatorRepository, IPublisherRepository publisherRepository)
    {
        _bookRepository = bookRepository;
        _authorRepository = authorRepository;
        _translatorRepository = translatorRepository;
        _publisherRepository = publisherRepository;
    }
    #region Create
    public async Task<OperationResult> Create(BookDto model)
    {
        OperationResult operationResult = new();

        // chk duplicate
        if (_bookRepository.Exists(x => x.Title == model.Title))
        {
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
        }

        // 2. add new book
        Book book = new(
            title: model.Title,
            iSBN: model.ISBN,
            code: model.Code,
            description: model.Description,
            categoryId: model.CategoryId);

        // 3. add Authors
        if (model.Authors != null && model.Authors.Any())
        {
            foreach (var authorName in model.Authors)
            {
                var author = await _authorRepository.GetByName(authorName);
                if (author != null)
                {
                    var bookAuthor = new BookAuthor
                    {
                        Book = book,
                        Author = author,
                        AuthorBookId = book.Id,
                        AuthorId = author.Id,
                    };
                    book.BookAuthors.Add(bookAuthor);
                }
                //else
            }
        }
        // 4. add Publishers

        if (model.Publishers != null && model.Publishers.Any())
        {
            foreach (var publisherName in model.Publishers)
            {
                var publisher = await _publisherRepository.GetByName(publisherName);
                if (publisher != null)
                {
                    var bookPublisher = new BookPublisher
                    {
                        PublisherBookId = publisher.Id,
                        Book = book,
                        PublisherId = publisher.Id,
                        Publisher = publisher
                    };
                    book.BookPublishers.Add(bookPublisher);
                }
                //else
            }
        }

        // 5. add Translators
        if (model.Translators != null && model.Translators.Any())
        {
            foreach (var translatorName in model.Translators)
            {
                var translator = await _translatorRepository.GetByName(translatorName);
                if (translator != null)
                {
                    var bookTranslator = new BookTranslator
                    {
                        TranslatorBookId = translator.Id,
                        Book = book,
                        TranslatorId = translator.Id,
                        Translator = translator
                    };
                    book.BookTranslators.Add(bookTranslator);
                }
                //else
            }
        }

        // 6. save in db
        var result = await _bookRepository.CreateAsync(book);

        if (result == null)
        {
            return operationResult.Failed(ApplicationMessages.ProblemFound);
        }

        return operationResult.Succeeded();
    }

    #endregion

    #region Read
    //public Task<List<BookViewModel>> GetAll()
    //{
    //    var result = _bookRepository.GetAll()
    //        .Select(book => new BookViewModel
    //        {
    //            Id = book.Id,
    //            Title = book.Title,
    //            ISBN = book.ISBN,
    //            Code = book.Code,
    //            Description = book.Description,
    //            AuthorId = book.AuthorId,
    //            PublisherId = book.PublisherId,
    //            TranslatorId = book.TranslatorId,
    //            CategoryId = book.CategoryId,
    //            Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
    //            Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
    //            Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
    //            Category = book.Category.Name,
    //        }).ToList();

    //    return Task.FromResult(result);
    //}

    public async Task<List<BookViewModel>> GetBooks()
    {
        return await _bookRepository.GetBooks();
    }

    public async Task<List<BookViewModel>> GetAllBooks()
    {
        var result = await _bookRepository.GetAll()
            .Include(book => book.Category)
            .Include(book => book.BookAuthors)
            .ThenInclude(ab => ab.Author)
            .Include(ap => ap.BookPublishers)
            .ThenInclude(ap => ap.Publisher)
            .Include(at => at.BookTranslators)
            .ThenInclude(at => at.Translator)
                    .Select(book => new BookViewModel
                    {
                        Id = book.Id,
                        Title = book.Title,
                        ISBN = book.ISBN,
                        Code = book.Code,
                        Description = book.Description,
                        CategoryId = book.CategoryId,
                        Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
                        Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
                        Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
                        Category = book.Category.Name,
                    }).ToListAsync();
        return result;
    }

    public async Task<UpdateBookViewModel> GetById(int id)
    {
        var book = await _bookRepository.GetByIdAsync(id);
        UpdateBookViewModel dto = new()
        {
            Id = id,
            Title = book.Title,
            ISBN = book.ISBN,
            Code = book.Code,
            Description = book.Description,
            CategoryId = book.CategoryId,
            Category = book.Category.Name,
            Authors = book.BookAuthors.Select(ba => ba.Author.Name).ToList(),
            Publishers = book.BookPublishers.Select(bp => bp.Publisher.Name).ToList(),
            Translators = book.BookTranslators.Select(bt => bt.Translator.Name).ToList(),
        };
        return dto;
    }
    #endregion

    #region Update
    public async Task<OperationResult> Update(UpdateBookViewModel dto)
    {
        OperationResult operationResult = new();
        var book = await _bookRepository.GetByIdAsync(dto.Id);
        if (book == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        if (_bookRepository.Exists(x => x.Title == dto.Title && x.Id != dto.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        book.Edit(dto.Title, dto.ISBN, dto.Code, dto.Description, dto.CategoryId);

        await _bookRepository.UpdateAsync(book);
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