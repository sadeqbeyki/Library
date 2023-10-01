using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.BookCategory;
using Microsoft.AspNetCore.Mvc;

namespace LibBook.EndPoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookCategoryController : ControllerBase
    {
        private readonly IBookCategoryService _bookCategoryService;

        public BookCategoryController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var bookCategory = _bookCategoryService.GetCategories();
            return Ok(bookCategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(int id)
        {
            var bookCategory = await _bookCategoryService.GetById(id);
            if (bookCategory == null)
                return NotFound();

            return Ok(bookCategory);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _bookCategoryService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<ActionResult> Update(int id, [FromBody] BookCategoryDto dto)
        {
            var bookCategory = await _bookCategoryService.Update(id, dto);

            return Ok(bookCategory);
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(int id)
        {
            await _bookCategoryService.Delete(id);
            return NoContent();
        }

        [HttpGet("CategoryBooks/{id}")]
        public async Task<ActionResult<List<BookDto>>> GetCategoryWithBooks(int id)
        {
            var books = await _bookCategoryService.GetCategoryWithBooks(id);
            if (books == null)
                return NoContent();
            return Ok(books);
        }
    }

}