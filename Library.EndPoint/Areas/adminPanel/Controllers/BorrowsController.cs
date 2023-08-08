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

    public async Task<ActionResult<List<BorrowDto>>> Index()
    {
        List<BorrowDto> borrows = await _borrowService.GetAllBorrows();
        return View("Index", borrows);
    }
    #region Create
    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Borrowing()
    {
        var command = new CreateBorrowViewModel
        {
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAllBooks(),
        };
        return View("Borrowing", command);
    }
    [HttpPost]
    public async Task<ActionResult> Borrowing(BorrowDto dto)
    {
        var result = await _borrowService.Borrowing(dto);
        if (result == null)
        {
            return View("Error");
        }
        return RedirectToAction("Index", result);
    }

    public async Task<ActionResult> BorrowingRegistration(int id)
    {
        await _borrowService.BorrowingRegistration(id);
        return RedirectToAction("Index");
    }
    #endregion

    #region Read
    [HttpGet]
    public async Task<ActionResult<BorrowDto>> Details(int id)
    {
        var result = await _borrowService.GetBorrowById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    [HttpPost, ActionName("Details")]
    [ValidateAntiForgeryToken]
    public async Task<ActionResult> Delete(int id)
    {
        await _borrowService.Delete(id);
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
            Books = await _bookService.GetAllBooks(),
        };

        return View("Update", model);
    }
    [HttpPut, HttpPost]
    public async Task<ActionResult> Update(UpdateBorrowViewModel model)
    {
        var result = await _borrowService.Update(model.Borrow);
        return RedirectToAction("Index", result);
    }
    #endregion

    [HttpGet]
    public async Task<ActionResult> ReturnBorrow(int id)
    {
        await _borrowService.ReturnBorrow(id);
        return RedirectToAction("Index");
    }

}
