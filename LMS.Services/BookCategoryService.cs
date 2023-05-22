using LMS.Contracts.BookCategory;
using LMS.Domain.BookCategoryAgg;


namespace LMS.Services
{
    public class BookCategoryService : IBookCategoryService
    {
        private readonly IBookCategoryRepository  _repository;

        public BookCategoryService(IBookCategoryRepository repository)
        {
            _repository = repository;
        }

        public async Task<BookCategoryDto> Create(BookCategoryDto dto)
        {
            var bookCategory = new BookCategory
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addBookCategory = await _repository.AddAsync(bookCategory);

            var result = new BookCategoryDto
            {
                Name = addBookCategory.Name,
                Description = addBookCategory.Description,
            };

            return result;
        }

        public Task<List<BookCategoryDto>> GetAll()
        {
            var result = _repository.GetAll()
                .Select(bookCategory => new BookCategoryDto
                {
                    Id = bookCategory.Id,
                    Name = bookCategory.Name,
                    Description = bookCategory.Description,
                }).ToList();

            return Task.FromResult(result);
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

        public async Task<BookCategoryDto> Update(Guid id, BookCategoryDto entity)
        {
            var existingBookCategory = await _repository.GetByIdAsync(id);
            if (existingBookCategory == null)
                return null;

            existingBookCategory.Name = entity.Name;
            existingBookCategory.Description = entity.Description;

            await _repository.UpdateAsync(existingBookCategory);
            return entity;
        }


    }
}