using LMS.Contracts.BookCategoryContract;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers
{
    [Area("adminPanel")]
    public class BookCategoriesController : Controller
    {
        private readonly IBookCategoryService _bookCategoryService;

        public BookCategoriesController(IBookCategoryService bookCategoryService)
        {
            _bookCategoryService = bookCategoryService;
        }

        public async Task<ActionResult<List<BookCategoryDto>>> Index()
        {
            var result = await _bookCategoryService.GetAll();
            return View(result);
        }
        [HttpGet]
        public async Task<ActionResult<BookCategoryDto>> Details(Guid id)
        {
            var result = await _bookCategoryService.GetById(id);
            if (result == null)
                return NotFound();
            return View(result);
        }
        [HttpGet]
        public ActionResult<BookCategoryDto> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(BookCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _bookCategoryService.Create(dto);
            return RedirectToAction("Index", result);
        }
        [HttpGet]
        public async Task<ActionResult<BookCategoryDto>> Update(Guid id)
        {
            var category = await _bookCategoryService.GetById(id);
            if (category != null)
            {
                return View(category);
            }
            return View(category);
        }
        [HttpPost]
        public async Task<ActionResult> Update(Guid id, BookCategoryDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _bookCategoryService.Update(id, dto);
            return RedirectToAction("Index", result);
        }
        [HttpGet]
        public async Task<ActionResult<BookCategoryDto>> Delete(Guid id)
        {
            var category = await _bookCategoryService.GetById(id);
            if (id == Guid.Empty || category == null)
            {
                return NoContent();
            }
            return View(category);
        }
        [HttpPost, ActionName("Delete")]
        [ValidateAntiForgeryToken]
        public async Task<ActionResult> ConfirmDelete(Guid id)
        {
            await _bookCategoryService.Delete(id);
            return RedirectToAction("Index");
        }
    }
}
