using LMS.Contracts.Book;
using LMS.Contracts.BookCategory;
using Microsoft.AspNetCore.Mvc;

namespace LMS.EndPoint.Controllers
{
    [ApiController]
    [Route("api/[controller]")]
    public class BookController : ControllerBase
    {
        private readonly IBookService _bookService;

        public BookController(IBookService bookService)
        {
            _bookService = bookService;
        }

        [HttpGet]
        public IActionResult GetAll()
        {
            var book = _bookService.GetAll();
            return Ok(book);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
        {
            var book = await _bookService.GetById(id);
            if (book == null)
                return NotFound();

            return Ok(book);
        }

        [HttpPost]
        public async Task<IActionResult> Create([FromBody] BookCategoryDto dto)
        {
            if (!ModelState.IsValid)
                return BadRequest(ModelState);
            var result = await _bookService.Create(dto);
            return Ok(result);
        }

        [HttpPut("{id}")]
        public async Task<BookCategoryDto> Update(Guid id, [FromBody] BookCategoryDto bookCategory)
        {
            var updatedBookCategory = await _bookService.Update(id, bookCategory);

            return updatedBookCategory;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookService.Delete(id);
            return NoContent();
        }
    }

}