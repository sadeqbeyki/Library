using LMS.Contracts.Book;
using LMS.Services;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers
{
    public class BooksController : Controller
    {
        private readonly BookService _bookService;

        public BooksController(BookService bookService)
        {
            _bookService = bookService;
        }

        public async Task<ActionResult<List<BookViewModel>>> Index()
        {
            var result = await _bookService.GetAll();
            return View(result);
        }
    }
}
