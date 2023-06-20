using LMS.Contracts.Book;
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
    }
}
