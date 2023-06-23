using LMS.Contracts.Book;
using LMS.Contracts.BookCategoryContract;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers
{
    [Area("adminPanel")]
    public class BooksController : Controller
    {
        private readonly IBookService _bookService;

        public BooksController(IBookService bookService)
        {
            _bookService = bookService;
        }
        [HttpGet]
        public async Task<ActionResult<List<BookViewModel>>> Index()
        {
            var result = await _bookService.GetAll();
            return View(result);
        }
        [HttpGet]
        public async Task<ActionResult<BookViewModel>> Details(Guid id)
        {
            var result = await _bookService.GetById(id);
            if (result == null)
                return NotFound();
            return View(result);
        }
        [HttpGet]
        public ActionResult<BookDto> Create()
        {
            return View();
        }
        [HttpPost]
        public async Task<ActionResult> Create(BookDto dto)
        {
            if (!ModelState.IsValid)
            {
                return View();
            }
            var result = await _bookService.Create(dto);
            return RedirectToAction("Index", result);
        }
    }
}
