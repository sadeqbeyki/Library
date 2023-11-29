using LibBook.DomainContracts.Book;
using LibBook.DomainContracts.Borrow;
using LibIdentity.DomainContracts.UserContracts;
using Library.EndPoint.Areas.adminPanel.Models;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace Library.EndPoint.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "admin, manager")]
public class BorrowsController : Controller
{
    private readonly ILoanService _loanService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;


    public BorrowsController(ILoanService borrowService, IBookService bookService, IUserService userService)
    {
        _loanService = borrowService;
        _bookService = bookService;
        _userService = userService;
    }

    #region Read
    public async Task<ActionResult<List<LoanDto>>> Index(LoanSearchModel searchModel/*, int? page*/)
    {
        //List<LoanDto> loans = _loanService.GetAll();
        LendViewModel model = new()
        {
            Loans = _loanService.Search(searchModel),
            SearchModel = searchModel,
            Books = new SelectList(await _bookService.GetBooks(), "Id", "Title").ToList(),
        };

        //int pageNumber = page ?? 1;
        //int pageSize = 6;
        //var pagedLog = model.ToPagedList(pageNumber, pageSize);

        return View(model);

        //return View("Index", pagedLog);
    }
    public async Task<ActionResult<List<LoanDto>>> PendingLoans(int? page)
    {
        List<LoanDto> loans = await _loanService.GetPendingLoans();
        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);
        return View("PendingLoans", pagedLog);
    }
    public ActionResult<List<LoanDto>> ApprovedLoans(int? page)
    {
        List<LoanDto> loans = _loanService.GetApprovedLoans();

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("ApprovedLoans", pagedLog);
    }
    public ActionResult<List<LoanDto>> ReturnedLoans(int? page)
    {
        List<LoanDto> loans = _loanService.GetReturnedLoans();

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("ReturnedLoans", pagedLog);
    }
    public async Task<ActionResult<List<LoanDto>>> OverdueLoans(int? page)
    {
        List<LoanDto> loans = await _loanService.GetOverdueLones();

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("OverdueLoans", pagedLog);
    }
    public ActionResult<List<LoanDto>> DeletedLoans(int? page)
    {
        List<LoanDto> loans = _loanService.GetDeletedLoans();

        int pageNumber = page ?? 1;
        int pageSize = 6;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("DeletedLoans", pagedLog);
    }

    [HttpGet]
    public async Task<ActionResult<LoanDto>> Details(int id)
    {
        var result = await _loanService.GetLoanById(id);
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
        var result = await _loanService.Lending(model);
        if (!result.IsSucceeded)
            return RedirectToAction("Lending", model);
        return RedirectToAction("PendingLoans", result);
    }

    public async Task<ActionResult> SubmitLend(int id)
    {
        var result = await _loanService.SubmitLend(id);
        if (result.IsSucceeded)
        {
            return RedirectToAction("PendingLoans");
        }
        return RedirectToAction("PendingLoans");
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<ActionResult<LoanDto>> Update(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _loanService.GetLoanById(id),
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };

        return View("Update", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Update(UpdateBorrowViewModel model)
    {
        var result = _loanService.Update(model.Borrow);
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
        _loanService.SoftDelete(model);
        return RedirectToAction("Index");
    }
    #endregion

    #region Return
    [HttpGet]
    public async Task<ActionResult<LoanDto>> Return(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Borrow = await _loanService.GetLoanById(id),
            Members = await _userService.GetUsers(),
            Books = await _bookService.GetAll(),
        };

        return View("Return", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Return(UpdateBorrowViewModel model)
    {
        var result = _loanService.Returning(model.Borrow);
        if (result.IsSucceeded)
            return RedirectToAction("ApprovedLoans", result);
        return RedirectToAction("Return", model);
    }
    #endregion


}
