using Identity.Application.Interfaces;
using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using Library.EndPoint.MVC.Helper;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.Rendering;
using X.PagedList;

namespace Library.EndPoint.MVC.Areas.adminPanel.Controllers;
[Area("adminPanel")]
[Authorize(Roles = "Admin, Manager")]
public class LoansController : Controller
{
    private readonly ILendService _lendService;
    private readonly IBookService _bookService;
    private readonly IUserService _userService;


    public LoansController(ILendService lendService, IBookService bookService, IUserService userService)
    {
        _lendService = lendService;
        _bookService = bookService;
        _userService = userService;
    }

    #region Search
    public async Task<ActionResult<List<LendDto>>> Index(LendSearchModel searchModel, int? page)
    {
        //List<LoanDto> loans = _loanService.GetAll();
        const int pageSize = 5;

        var loans = _lendService.Search(searchModel);

        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);

        LendViewModel model = new()
        {
            Loans = paginatedLoans,
            SearchModel = searchModel,
            Books = new SelectList(await _bookService.GetBooks(), "Id", "Title").ToList(),
        };

        return View(model);
    }
    #endregion

    #region Read
    public async Task<ActionResult<List<LendDto>>> PendingLoans(int? page)
    {
        List<LendDto> loans = await _lendService.GetPendingLoans();

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("PendingLoans", paginatedLoans);
    }
    public ActionResult<List<LendDto>> ApprovedLoans(int? page)
    {
        List<LendDto> loans = _lendService.GetApprovedLoans();

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("ApprovedLoans", paginatedLoans);
    }
    public ActionResult<List<LendDto>> ReturnedLoans(int? page)
    {
        List<LendDto> loans = _lendService.GetReturnedLoans();

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("ReturnedLoans", paginatedLoans);
    }
    public async Task<ActionResult<List<LendDto>>> OverdueLoans(int? page)
    {
        List<LendDto> loans = await _lendService.GetOverdueLones();

        int pageSize = 5;
        var pagedLog = loans.ToPagedList(page ?? 1, pageSize);

        return View("OverdueLoans", pagedLog);
    }
    public ActionResult<List<LendDto>> DeletedLoans(int? page)
    {
        List<LendDto> loans = _lendService.GetDeletedLoans();

        int pageNumber = page ?? 1;
        int pageSize = 5;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("DeletedLoans", pagedLog);
    }

    [HttpGet]
    public async Task<ActionResult<LendDto>> Details(int id)
    {
        var result = await _lendService.GetLendById(id);
        if (result == null)
            return NotFound();
        return View(result);
    }
    #endregion

    #region Create
    [HttpGet]
    public async Task<ActionResult<LendDto>> Lending()
    {
        var command = new CreateBorrowViewModel
        {
            Members = await _userService.GetAllUsersAsync(),
            Books = await _bookService.GetAll(),
        };
        return View("Lending", command);
    }
    [HttpPost]
    public async Task<ActionResult> Lending(LendDto model)
    {
        var result = await _lendService.Lending(model);
        if (!result.IsSucceeded)
            return RedirectToAction("Lending", model);
        return RedirectToAction("PendingLoans", result);
    }

    public async Task<ActionResult> SubmitLend(int id)
    {
        var result = await _lendService.SubmitLend(id);
        if (result.IsSucceeded)
        {
            return RedirectToAction("PendingLoans");
        }
        return RedirectToAction("PendingLoans");
    }
    #endregion

    #region Update
    [HttpGet]
    public async Task<ActionResult<LendDto>> Update(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Lend = await _lendService.GetLendById(id),
            Members = await _userService.GetAllUsersAsync(),
            Books = await _bookService.GetAll(),
        };

        return View("Update", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Update(UpdateBorrowViewModel model)
    {
        var result = _lendService.Update(model.Lend);
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
    //    await _lendService.Delete(id);
    //    return RedirectToAction("Index");
    //}
    [HttpPost, ActionName("Details")]
    [ValidateAntiForgeryToken]
    public IActionResult SoftDelete(LendDto model)
    {
        _lendService.SoftDelete(model);
        return RedirectToAction("Index");
    }
    #endregion

    #region Return
    [HttpGet]
    public async Task<ActionResult<LendDto>> Return(int id)
    {
        var model = new UpdateBorrowViewModel
        {
            Lend = await _lendService.GetLendById(id),
            Members = await _userService.GetAllUsersAsync(),
            Books = await _bookService.GetAll(),
        };

        return View("Return", model);
    }
    [HttpPut, HttpPost]
    public IActionResult Return(UpdateBorrowViewModel model)
    {
        var result = _lendService.Returning(model.Lend);
        if (result.IsSucceeded)
            return RedirectToAction("ApprovedLoans", result);
        return RedirectToAction("Return", model);
    }
    #endregion

    //public class PaginatedList<T>
    //{
    //    public int PageIndex { get; private set; }
    //    public int TotalPages { get; private set; }
    //    public int TotalCount { get; private set; }
    //    public List<T> Items { get; private set; }

    //    public PaginatedList(List<T> items, int count, int pageIndex, int pageSize)
    //    {
    //        PageIndex = pageIndex;
    //        TotalPages = (int)Math.Ceiling(count / (double)pageSize);
    //        TotalCount = count;
    //        Items = items.Skip((pageIndex - 1) * pageSize).Take(pageSize).ToList();
    //    }

    //    public bool HasPreviousPage
    //    {
    //        get { return PageIndex > 1; }
    //    }

    //    public bool HasNextPage
    //    {
    //        get { return PageIndex < TotalPages; }
    //    }

    //    public static PaginatedList<T> Create(List<T> source, int pageIndex, int pageSize)
    //    {
    //        int count = source.Count;
    //        return new PaginatedList<T>(source, count, pageIndex, pageSize);
    //    }
    //}
}
