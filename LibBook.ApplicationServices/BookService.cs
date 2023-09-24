﻿using AppFramework.Application;
using LibBook.Domain;
using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Book;
using Microsoft.EntityFrameworkCore;

namespace LibBook.ApplicationServices;

public class BookService : IBookService
{
    private readonly IBookRepository _bookRepository;
    private readonly IAuthorRepository _authorRepository;

    public BookService(IBookRepository bookRepository)
    {
        _bookRepository = bookRepository;
    }
    #region Create
    public async Task<OperationResult> Create(CreateBookDto bookDto)
    {
        OperationResult operationResult = new();

        // chk duplicate
        if (_bookRepository.Exists(x => x.Title == bookDto.Title))
        {
            return operationResult.Failed(ApplicationMessages.DuplicatedRecord);
        }

        // 2. add new book
        Book book = new(
            title: bookDto.Title,
            iSBN: bookDto.ISBN,
            code: bookDto.Code,
            description: bookDto.Description,
            categoryId: bookDto.CategoryId);

        // 3. add authors
        if (bookDto.Authors != null && bookDto.Authors.Any())
        {
            foreach (var authorName in bookDto.Authors)
            {
                var author = await _authorRepository.GetByName(authorName);
                if (author != null)
                {
                    book.BookAuthors.Add(new BookAuthor { Author = author });
                }
                //else
            }
        }

        // 4. save in db
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
                        AuthorId = book.AuthorId,
                        PublisherId = book.PublisherId,
                        TranslatorId = book.TranslatorId,
                        CategoryId = book.CategoryId,
                        Authors = book.BookAuthors.Select(ab => ab.Author.Name).ToList(),
                        Publishers = book.BookPublishers.Select(ab => ab.Publisher.Name).ToList(),
                        Translators = book.BookTranslators.Select(ab => ab.Translator.Name).ToList(),
                        Category = book.Category.Name,
                    }).ToListAsync();
        return result;
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
    #endregion

    #region Update
    public async Task<OperationResult> Update(BookViewModel dto)
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