using LibBook.Domain.AuthorAgg;
using LibBook.Domain.BookAgg;
using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;

namespace LibBook.ApplicationServices
{
    public class AuthorService : IAuthorService
    {
        private readonly IAuthorRepository _authorRepository;

        private readonly IBookRepository _bookRepository;

        public AuthorService(IAuthorRepository autherRepository, IBookRepository bookRepository)
        {
            _authorRepository = autherRepository;
            _bookRepository = bookRepository;
        }

        public async Task<AuthorDto> Create(AuthorDto dto)
        {
            var auther = new Author
            {
                Name = dto.Name,
                Description = dto.Description,
            };
            var addAuther = await _authorRepository.CreateAsync(auther);

            var result = new AuthorDto
            {
                Name = addAuther.Name,
                Description = addAuther.Description,
            };

            return result;
        }

        public async Task<List<AuthorDto>> GetAll()
        {
            var result = _authorRepository.GetAll()
                .Select(author => new AuthorDto
                {
                    Id = author.Id,
                    Name = author.Name,
                    Description = author.Description,
                }).ToList();

            return await Task.FromResult(result);
        }

        public async Task<AuthorDto> GetById(int id)
        {
            var result = await _authorRepository.GetByIdAsync(id);
            AuthorDto dto = new()
            {
                Id = id,
                Name = result.Name,
                Description = result.Description,
            };
            return dto;
        }

        public async Task Delete(int id)
        {
            var result = await _authorRepository.GetByIdAsync(id);
            await _authorRepository.DeleteAsync(result);
        }

        public async Task<AuthorDto> Update(int id, AuthorDto entity)
        {
            var existingAuthor = await _authorRepository.GetByIdAsync(id);
            if (existingAuthor == null)
                return null;

            existingAuthor.Name = entity.Name;
            existingAuthor.Description = entity.Description;

            await _authorRepository.UpdateAsync(existingAuthor);
            return entity;
        }

        public async Task<List<BookViewModel>> GetAuthorBooks(int id)
        {
            var books = await _authorRepository.GetAuthorBooks(id);
            var result = books.Select(b => new BookViewModel
            {
                Title = b.Title,
                ISBN = b.ISBN,
                Code = b.Code,
                Description = b.Description,
                CategoryId = b.CategoryId,
            }).ToList();
            return result;
        }

        public Task<List<AuthorDto>> GetAuthors()
        {
            return _authorRepository.GetAuthors();
        }
    }
}