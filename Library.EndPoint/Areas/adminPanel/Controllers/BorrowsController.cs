using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.Borrow;
using LibIdentity.DomainContracts.UserContracts;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using X.PagedList;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "admin, manager")]
public class BorrowsController : Controller
{
    private readonly ILoanService _borrowService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;


    public BorrowsController(ILoanService borrowService, IBookService bookService, IUserService userService)
    {
        _borrowService = borrowService;
        _bookService = bookService;
        _userService = userService;
    }

    #region Read
    public async Task<ActionResult<List<LoanDto>>> Index()
    {
        List<LoanDto> loans = await _borrowService.GetAll();
        return View("Index", loans);
    }
    public async Task<ActionResult<List<LoanDto>>> PendingLoans(int? page)
    {
        List<LoanDto> loans = await _borrowService.GetPendingLoans();
        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);
        return View("PendingLoans", pagedLog);
    }
    public ActionResult<List<LoanDto>> ApprovedLoans()
    {
        List<LoanDto> loans = _borrowService.GetApprovedLoans();
        return View("ApprovedLoans", loans);
    }
    public ActionResult<List<LoanDto>> ReturnedLoans()
    {
        List<LoanDto> loans = _borrowService.GetReturnedLoans();
        return View("ReturnedLoans", loans);
    }
    public async Task<ActionResult<List<LoanDto>>> OverdueLoans(int? page)
    {
        List<LoanDto> loans = await _borrowService.GetOverdueLones();

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("OverdueLoans", pagedLog);
    }
    public ActionResult<List<LoanDto>> DeletedLoans()
    {
        List<LoanDto> loans = _borrowService.GetDeletedLoans();
        return View("DeletedLoans", loans);
    }

    [HttpGet]
    public async Task<ActionResult<LoanDto>> Details(int id)
    {
        var result = await _borrowService.GetLoanById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    #endregion

    #region Create
    [HttpGet]
    public async Task<ActionResult<LoanDto>> Lending()
    {
        var command = new CreateBorrowViewModel
        {
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };
        return View("Lending", command);
    }
    [HttpPost]
    public async Task<ActionResult> Lending(LoanDto model)
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
    public async Task<ActionResult<LoanDto>> Update(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _borrowService.GetLoanById(id),
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
    public IActionResult SoftDelete(LoanDto model)
    {
        _borrowService.SoftDelete(model);
        return RedirectToAction("Index");
    }
    #endregion

    #region Return
    [HttpGet]
    public async Task<ActionResult<LoanDto>> Return(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _borrowService.GetLoanById(id),
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
