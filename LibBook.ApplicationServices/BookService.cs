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
    public async Task<OperationResult> Create(BookDto inputModel)
    {
        OperationResult operationResult = new();

        // chk duplicate
        if (_bookRepository.Exists(x => x.Title == inputModel.Title))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        //var bookCategory = _bookCategoryRepository.GetByIdAsync(model.CategoryId);
        // 2. add new book
        Book book = new(inputModel.Title, inputModel.ISBN, inputModel.Code, inputModel.Description, inputModel.CategoryId);

        // 3. add Authors
        if (inputModel.Authors != null && inputModel.Authors.Any())
        {
            foreach (var authorName in inputModel.Authors)
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
        // 4. add Publishers
        if (inputModel.Publishers != null && inputModel.Publishers.Any())
        {
            foreach (var publisherName in inputModel.Publishers)
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
        // 5. add Translators
        if (inputModel.Translators != null && inputModel.Translators.Any())
        {
            foreach (var translatorName in inputModel.Translators)
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

        // 6. save in db
        var result = await _bookRepository.CreateAsync(book);

        if (result == null)
        {
            return operationResult.Failed(ApplicationMessages.ProblemFound);
        }

        return operationResult.Succeeded();
    }
    //private async Task<AuthorDto> AddAuthors(BookDto model, Book book)
    //{
    //    if (model.Authors != null && model.Authors.Any())
    //    {
    //        foreach (var authorName in model.Authors)
    //        {
    //            var author = await _authorRepository.GetByName(authorName);
    //            if (author != null)
    //            {
    //                var bookAuthor = new BookAuthor
    //                {
    //                    AuthorBookId = book.Id,
    //                    Book = book,
    //                    AuthorId = author.Id,
    //                    Author = author,
    //                };
    //                book.BookAuthors.Add(bookAuthor);
    //            }
    //            //else
    //        }
    //    }
    //}

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

                Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
                Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
                Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
                Category = book.Category.Name,
            }).FirstOrDefaultAsync(b => b.Id == id);

        return result;

        //var book = await _bookRepository.GetByIdAsync(id);
        //BookViewModel result = new()
        //{
        //    Id = id,
        //    Title = book.Title,
        //    ISBN = book.ISBN,
        //    Code = book.Code,
        //    Description = book.Description,
        //    CategoryId = book.CategoryId,
        //    //Category = book.Category.Name,
        //    Category = "salam",
        //    Authors = book.BookAuthors.Select(ba => ba.Author.Name).ToList(),
        //    Publishers = book.BookPublishers.Select(bp => bp.Publisher.Name).ToList(),
        //    Translators = book.BookTranslators.Select(bt => bt.Translator.Name).ToList(),
        //};
        //return result;


    }
    #endregion

    #region Update

    public async Task<OperationResult> Update(BookViewModel model)
    {
        OperationResult operationResult = new();

        // 1. Check if the book exists
        var existingBook = await _bookRepository.GetByIdAsync(model.Id);
        if (existingBook == null)
        {
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        }

        // 1.1. Check if duplicate found
        if (_bookRepository.Exists(x => x.Title == model.Title && x.Id != model.Id))
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

        // 2. Update the book properties
        existingBook.Edit(model.Title, model.ISBN, model.Code, model.Description, model.CategoryId);

        // 3. Clear the existing relationships with authors, publishers, and translators
        existingBook.BookAuthors.Clear();
        existingBook.BookPublishers.Clear();
        existingBook.BookTranslators.Clear();

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
                        AuthorBookId = existingBook.Id,
                        Book = existingBook,
                        AuthorId = author.Id,
                        Author = author,
                    };
                    existingBook.BookAuthors.Add(bookAuthor);
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
                        PublisherBookId = existingBook.Id,
                        Book = existingBook,
                        PublisherId = publisher.Id,
                        Publisher = publisher
                    };
                    existingBook.BookPublishers.Add(bookPublisher);
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
                        TranslatorBookId = existingBook.Id,
                        Book = existingBook,
                        TranslatorId = translator.Id,
                        Translator = translator
                    };
                    existingBook.BookTranslators.Add(bookTranslator);
                }
            }
        }

        // 7. Update the book in the database
        var result = _bookRepository.UpdateAsync(existingBook);
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