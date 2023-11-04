using AppFramework.Infrastructure;
using Humanizer;
using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.Borrow;
using LibIdentity.DomainContracts.UserContracts;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "admin, manager")]
public class BorrowsController : Controller
{
    private readonly IBorrowService _borrowService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;


    public BorrowsController(IBorrowService borrowService, IBookService bookService, IUserService userService)
    {
        _borrowService = borrowService;
        _bookService = bookService;
        _userService = userService;
    }

    #region Read
    public async Task<ActionResult<List<BorrowDto>>> Index()
    {
        List<BorrowDto> loans = await _borrowService.GetAllLoans();
        return View("Index", loans);
    }
    public ActionResult<List<BorrowDto>> ApprovedLoans()
    {
        List<BorrowDto> loans = _borrowService.GetApprovedLoans();
        return View("ApprovedLoans", loans);
    }

    public ActionResult<List<BorrowDto>> ReturnedLoans()
    {
        List<BorrowDto> loans = _borrowService.GetReturnedLoans();
        return View("ReturnedLoans", loans);
    }

    public ActionResult<List<BorrowDto>> DeletedLoans()
    {
        List<BorrowDto> loans = _borrowService.GetDeletedLoans();
        return View("DeletedLoans", loans);
    }

    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Details(int id)
    {
        var result = await _borrowService.GetBorrowById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    #endregion

    #region Create
    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Lending()
    {
        var command = new CreateBorrowViewModel
        {
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };
        return View("Lending", command);
    }
    [HttpPost]
    public async Task<ActionResult> Lending(BorrowDto model)
    {
        var result = await _borrowService.Lending(model);
        if (!result.IsSucceeded)
            return RedirectToAction("Lending", model);
        return RedirectToAction("Index", result);
    }

    public async Task<ActionResult> SubmitLend(int id)
    {
        var result = await _borrowService.SubmitLend(id);
        if (result.IsSucceeded)
        {
            return RedirectToAction("Index");
        }
        return RedirectToAction("Index");
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Update(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _borrowService.GetBorrowById(id),
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };

        return View("Update", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Update(UpdateBorrowViewModel model)
    {
        var result = _borrowService.Update(model.Borrow);
        if (result.IsSucceeded)
            return RedirectToAction("Index", result);
        return RedirectToAction("Update", model);
    }
    #endregion

    #region Delete
    //[HttpPost, ActionName("Details")]
    //[ValidateAntiForgeryToken]
    //public async Task<ActionResult> Delete(int id)
    //{
    //    await _borrowService.Delete(id);
    //    return RedirectToAction("Index");
    //}
    [HttpPost, ActionName("Details")]
    [ValidateAntiForgeryToken]
    public IActionResult SoftDelete(BorrowDto model)
    {
        _borrowService.SoftDelete(model);
        return RedirectToAction("Index");
    }
    #endregion

    #region Return
    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Return(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _borrowService.GetBorrowById(id),
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };

        return View("Return", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Return(UpdateBorrowViewModel model)
    {
        var result = _borrowService.Returning(model.Borrow);
        if (result.IsSucceeded)
            return RedirectToAction("Index", result);
        return RedirectToAction("Return", model);
    }
    #endregion


}
