using Library.Application.Contracts;
using Library.Application.DTOs.Books;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.Common;
using Microsoft.AspNetCore.Http;

namespace Library.Persistance.Services;

public class BookService : IBookService
{

    private readonly IAuthorRepository _authorRepository;
    private readonly ITranslatorRepository _translatorRepository;
    private readonly IPublisherRepository _publisherRepository;

    public BookService(
        IAuthorRepository authorRepository,
        ITranslatorRepository translatorRepository,
        IPublisherRepository publisherRepository)
    {
        _authorRepository = authorRepository;
        _translatorRepository = translatorRepository;
        _publisherRepository = publisherRepository;
    }


    public async Task<byte[]?> ConvertImageToByte(IFormFile Image)
    {
        if (Image != null && Image.Length > 0)
        {
            using var memoryStream = new MemoryStream();
            await Image.CopyToAsync(memoryStream);
            byte[]? picture = memoryStream.ToArray();
            return picture;
        }
        return null;
    }

    public async Task AddAuthors(BookDto model, Book book)
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
    public async Task AddPublishers(BookDto model, Book book)
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
    public async Task AddTranslators(BookDto model, Book book)
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

}