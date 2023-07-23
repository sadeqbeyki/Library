using LibBook.DomainContracts.Author;
using LibBook.DomainContracts.Book;
using Microsoft.AspNetCore.Mvc;

namespace LibBook.EndPoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class AuthorController : ControllerBase
    {
        private readonly IAuthorService _authorService;

        public AuthorController(IAuthorService authorService)
        {
            _authorService = authorService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var authors = _authorService.GetAll();
            return Ok(authors);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var author = await _authorService.GetById(id);
            if (author == null)
                return NotFound();

            return Ok(author);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] AuthorDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _authorService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<AuthorDto> Update(int id, [FromBody] AuthorDto author)
        {
            var authorPublisher = await _authorService.Update(id, author);

            return authorPublisher;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _authorService.Delete(id);
            return NoContent();
        }
        [HttpGet("AuthorBooks/{id}")]
        public async Task<ActionResult<List<BookDto>>> GetAuthorBooks(int id)
        {
            var books = await _authorService.GetAuthorBooks(id);
            if (books == null)
                return NoContent();
            return Ok(books);
        }
    }

}