using AppFramework.Application;
using Library.Application.Contracts;
using Library.Application.DTOs.Book;
using Library.Application.DTOs.BookCategory;
using Library.Application.Interfaces;
using Library.Domain.Entities.BookCategoryAgg;


namespace Library.Persistance.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository _repository;

        public BookCategoryService(IBookCategoryRepository repository)
        {
            _repository = repository;
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

        public async Task<List<BookCategoryDto>> GetCategories()
        {
            return await _repository.GetCategories();
        }

        public async Task<BookCategoryDto> GetById(int id)
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

        public async Task<bool> Delete(int id)
        {
            var category = await _repository.GetByIdAsync(id);
            var result =  _repository.DeleteAsync(category);
            return result.IsCompletedSuccessfully;
        }

        public async Task<OperationResult> Update(int id, BookCategoryDto dto)
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

        public async Task<List<BookDto>> GetCategoryWithBooks(int id)
        {
            var books = await _repository.GetCategoryWithBooks(id);
            var result = books.Select(b => new BookDto
            {
                Title = b.Title,
                ISBN = b.ISBN,
                Code = b.Code,
                Description = b.Description,
                CategoryId = b.CategoryId,
            }).ToList();
            return result;
        }
    }
}