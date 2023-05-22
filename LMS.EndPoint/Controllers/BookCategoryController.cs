using LMS.Contracts.BookCategory;
using Microsoft.AspNetCore.Mvc;

namespace LMS.EndPoint.Controllers
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
            var bookCategory = _bookCategoryService.GetAll();
            return Ok(bookCategory);
        }

        [HttpGet("{id}")]
        public async Task<IActionResult> GetById(Guid id)
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
        public async Task<BookCategoryDto> Update(Guid id, [FromBody] BookCategoryDto bookCategory)
        {
            var updatedBookCategory = await _bookCategoryService.Update(id, bookCategory);

            return updatedBookCategory;
        }

        [HttpDelete("{id}")]
        public async Task<IActionResult> Delete(Guid id)
        {
            await _bookCategoryService.Delete(id);
            return NoContent();
        }
    }

}