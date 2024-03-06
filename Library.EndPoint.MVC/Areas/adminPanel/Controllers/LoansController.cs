using Identity.Application.Interfaces;
using Library.Application.Contracts;
using Library.Application.CQRS.Queries.Book;
using Library.Application.CQRS.Queries.Lend;
using Library.Application.DTOs.Lend;
using Library.EndPoint.MVC.Areas.adminPanel.Models;
using Library.EndPoint.MVC.Helper;
using MediatR;
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
    private readonly IMediator _mediator;


    public LoansController(ILendService lendService, IBookService bookService, IUserService userService, IMediator mediator)
    {
        _lendService = lendService;
        _bookService = bookService;
        _userService = userService;
        _mediator = mediator;
    }

    #region Search
    public async Task<ActionResult<List<LendDto>>> Index(LendSearchModel searchModel, int? page)
    {
        const int pageSize = 5;

        var loans = await _mediator.Send(new SearchLendQuery(searchModel));

        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);

        LendViewModel model = new()
        {
            Loans = paginatedLoans,
            SearchModel = searchModel,
            Books = new SelectList(await _mediator.Send(new GetBooksQuery()), "Id", "Title").ToList(),
        };

        return View(model);
    }
    #endregion

    #region Read
    public async Task<ActionResult<List<LendDto>>> PendingLoans(int? page)
    {
        List<LendDto> loans = await _mediator.Send(new GetPendingLoansQuery());

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("PendingLoans", paginatedLoans);
    }
    public async Task<ActionResult<List<LendDto>>> ApprovedLoans(int? page)
    {
        List<LendDto> loans = await _mediator.Send(new GetApprovedLoansQuery());

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("ApprovedLoans", paginatedLoans);
    }
    public async Task<ActionResult<List<LendDto>>> ReturnedLoans(int? page)
    {
        List<LendDto> loans = await _mediator.Send(new GetReturnedLoansQuery());

        const int pageSize = 5;
        var paginatedLoans = PaginatedList<LendDto>.Create(loans, page ?? 1, pageSize);
        return View("ReturnedLoans", paginatedLoans);
    }
    public async Task<ActionResult<List<LendDto>>> OverdueLoans(int? page)
    {
        List<LendDto> loans = await _mediator.Send(new GetOverdueLonesQuery());

        int pageSize = 5;
        var pagedLog = loans.ToPagedList(page ?? 1, pageSize);

        return View("OverdueLoans", pagedLog);
    }
    public async Task<ActionResult<List<LendDto>>> DeletedLoans(int? page)
    {
        List<LendDto> loans = await _mediator.Send(new GetDeletedLoansQuery());

        int pageNumber = page ?? 1;
        int pageSize = 5;
        var pagedLog = loans.ToPagedList(pageNumber, pageSize);

        return View("DeletedLoans", pagedLog);
    }

    [HttpGet]
    public async Task<ActionResult<LendDto>> Details(int id)
    {
        var result = await _mediator.Send(new GetLendByIdQuery(id));
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
            //todo: call "GetAllUsersAsync" with api or acl
            Members = await _userService.GetAllUsersAsync(),
            Books = await _mediator.Send(new GetAllBooksQuery()),
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
}
