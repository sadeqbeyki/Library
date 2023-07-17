﻿using AppFramework.Application;
using AutoMapper;
using LMS.Contracts.Book;
using LMS.Contracts.BookCategoryContract;
using LMS.Domain.BookCategoryAgg;


namespace LMS.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _repository;
        private readonly IMapper _mapper;

        public BookCategoryService(IBookCategoryRepository repository, IMapper mapper)
        {
            _repository = repository;
            _mapper = mapper;
        }

        public async Task<OperationResult> Create(BookCategoryDto dto)
        {
            OperationResult operationResult = new();
            if (_repository.Exists(x => x.Name == dto.Name))
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

            BookCategory bookCategory = new(dto.Name, dto.Description);
            await _repository.CreateAsync(bookCategory);
            return operationResult.Succeeded();
        }

        public async Task<List<BookCategoryDto>> GetAll()
        {
            //List<BookCategory> categories = await _repository.GetAll().Take(50).ToListAsync();
            //return _mapper.Map<List<BookCategoryDto>>(categories);
            var result = _repository.GetAll()
                .Select(bookCategory => new BookCategoryDto
                {
                    Id = bookCategory.Id,
                    Name = bookCategory.Name,
                    Description = bookCategory.Description,
                }).ToList();

            return await Task.FromResult(result);
        }
        public async Task<List<BookCategoryDto>> GetCategories()
        {
            return await _repository.GetCategories();
        }

        public async Task<BookCategoryDto> GetById(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            BookCategoryDto dto = new()
            {
                Id = id,
                Name = result.Name,
                Description = result.Description,
            };
            return dto;
        }

        public async Task Delete(Guid id)
        {
            var result = await _repository.GetByIdAsync(id);
            await _repository.DeleteAsync(result);
        }

        public async Task<OperationResult> Update(Guid id, BookCategoryDto dto)
        {
            OperationResult operationResult = new();
            var bookCategory = await _repository.GetByIdAsync(id);
            if (bookCategory == null)
                return operationResult.Failed(ApplicationMessages.RecordNotFound);

            if (_repository.Exists(x => x.Name == dto.Name && x.Id != dto.Id))
                return operationResult.Failed(ApplicationMessages.DuplicatedRecord);

            bookCategory.Edit(dto.Name, dto.Description);
            await _repository.UpdateAsync(bookCategory);
            return operationResult.Succeeded();
        }

        public async Task<List<BookDto>> GetCategoryWithBooks(Guid id)
        {
            var books = await _repository.GetCategoryWithBooks(id);
            var result = books.Select(b => new BookDto
            {
                Title = b.Title,
                ISBN = b.ISBN,
                Code = b.Code,
                Description = b.Description,
                PublisherId = b.PublisherId,
                AuthorId = b.AuthorId,
                TranslatorId = b.TranslatorId,
                CategoryId = b.CategoryId,
            }).ToList();
            return result;
        }
    }
}