using AppFramework.Application;
using AutoMapper;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibBook.DomainContracts.Borrow;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibBook.ApplicationServices;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _borrowRepository;
    private readonly IMapper _mapper;

    private readonly IBookRepository _bookRepository;

    private readonly ILibraryInventoryAcl _inventoryAcl;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    private readonly IHttpContextAccessor _contextAccessor;


    public LoanService(ILoanRepository borrowRepository, IMapper mapper, ILibraryInventoryAcl inventoryAcl,
        IHttpContextAccessor contextAccessor, ILibraryIdentityAcl identityAcl, IBookRepository bookRepository)
    {
        _borrowRepository = borrowRepository;
        _mapper = mapper;

        _inventoryAcl = inventoryAcl;
        _contextAccessor = contextAccessor;
        _IdentityAcl = identityAcl;
        _bookRepository = bookRepository;
    }

    #region Create
    public async Task<OperationResult> Lending(LoanDto model)
    {
        OperationResult operationResult = new();
        if (model.BookId <= 0 || model.MemberId == null)
            return operationResult.Failed(ApplicationMessages.ModelIsNull);
        Borrow borrow = new(
            model.BookId,
            model.MemberId,
            GetCurrentOperatorId(),
            model.IdealReturnDate,
            model.ReturnEmployeeId,
            model.ReturnDate,
            model.Description
            );

        var result = await _borrowRepository.CreateAsync(borrow);
        return operationResult.Succeeded();
    }

    public async Task<OperationResult> SubmitLend(int lendId)
    {
        OperationResult operationResult = new();

        Borrow lend = await _borrowRepository.GetByIdAsync(lendId);
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        //borrow duplicate book check
        var memberDuplicateLoans = await _borrowRepository.GetDuplicatedLoans(lend.MemberID, lend.BookId);
        if (memberDuplicateLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.DuplicateLendByMember);

        //Member Overdue Loans
        var memberOverdueLoans = await _borrowRepository.GetMemberOverdueLoans(lend.MemberID);
        if (memberOverdueLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.MemberDidntReturnedTheBook);

        if (_inventoryAcl.LoanFromInventory(lend) == true)
        {
            lend.IsApproved = true;
            _borrowRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
    #endregion

    #region Read
    public List<LoanDto> GetAll()
    {
        var loans = _borrowRepository.GetAll().ToList();
        return _mapper.Map<List<LoanDto>>(loans);
    }

    public async Task<List<LoanDto>> GetPendingLoans()
    {
        var loans = await _borrowRepository.GetAll().Where(x => !x.IsDeleted && !x.IsApproved).ToListAsync();

        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = lend.MemberID,
            MemberName = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = lend.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            Description = lend.Description,
        }).ToList();

        return result;
    }

    public List<LoanDto> GetApprovedLoans()
    {
        var loans = _borrowRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && !x.IsReturned).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = lend.MemberID,
            MemberName = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = lend.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            Description = lend.Description
        }).ToList();
        return result;
    }

    public List<LoanDto> GetReturnedLoans()
    {
        var loans = _borrowRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && x.IsReturned).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnEmployeeId = _IdentityAcl.GetUserName(lend.ReturnEmployeeID).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public List<LoanDto> GetDeletedLoans()
    {
        var loans = _borrowRepository.GetAll().Where(x => x.IsDeleted).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnEmployeeId = _IdentityAcl.GetUserName(lend.ReturnEmployeeID).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        return await _borrowRepository.GetBorrowsByEmployeeId(employeeId);
    }

    public async Task<List<LoanDto>> GetBorrowsByMemberId(string memberId)
    {
        return await _borrowRepository.GetBorrowsByMemberId(memberId);
    }

    public async Task<LoanDto> GetLoanById(int borrowId)
    {
        Borrow borrow = await _borrowRepository.GetByIdAsync(borrowId);
        var book = await _bookRepository.GetByIdAsync(borrow.BookId);

        LoanDto dto = new()
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            BookTitle = book.Title,
            MemberId = borrow.MemberID,
            MemberName = await _IdentityAcl.GetUserName(borrow.MemberID),
            EmployeeId = borrow.EmployeeId,
            EmployeeName = await _IdentityAcl.GetUserName(borrow.EmployeeId),
            CreationDate = borrow.CreationDate,
            IdealReturnDate = borrow.IdealReturnDate,
            ReturnDate = borrow.ReturnDate,
            ReturnEmployeeId = await _IdentityAcl.GetUserName(borrow.ReturnEmployeeID),
            Description = borrow.Description,
        };
        return dto;
    }

    public async Task<List<LoanDto>> GetOverdueLones()
    {
        //return await _borrowRepository.GetOverdueLones();
        var loans = _borrowRepository.GetAll()
            .Where(b => b.IsApproved && b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = lend.MemberID,
            MemberName = _IdentityAcl.GetUserName(lend.MemberID).Result,
            EmployeeId = lend.EmployeeId,
            EmployeeName = _IdentityAcl.GetUserName(lend.EmployeeId).Result,
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            Description = lend.Description
        }).ToList();
        return result;
    }
    #endregion

    #region Update
    public OperationResult Update(LoanDto dto)
    {
        OperationResult operationResult = new();

        var borrow = _borrowRepository.GetByIdAsync(dto.Id).Result;
        if (borrow == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        borrow.Edit(
            dto.BookId,
            dto.MemberId,
            dto.EmployeeId,
            dto.IdealReturnDate,
            dto.ReturnEmployeeId,
            dto.ReturnDate,
            dto.Description);

        _borrowRepository.UpdateAsync(borrow);
        return operationResult.Succeeded();
    }

    #endregion

    #region Return
    public OperationResult Returning(LoanDto dto)
    {
        OperationResult operationResult = new();

        var lend = _borrowRepository.GetByIdAsync(dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        lend.Edit(dto.BookId, dto.MemberId, dto.EmployeeId, dto.IdealReturnDate, GetCurrentOperatorId(), DateTime.Now, dto.Description);

        if (ReturnLoan(dto.Id).IsSucceeded)
        {
            _borrowRepository.UpdateAsync(lend);
            return operationResult.Succeeded();
        }
        else
        {
            return operationResult.Failed(ApplicationMessages.ReturnFailed);
        }
    }

    private OperationResult ReturnLoan(int lendId)
    {
        OperationResult operationResult = new();
        Borrow lend = _borrowRepository.GetByIdAsync(lendId).Result;
        if (lend == null)
            operationResult.Failed(ApplicationMessages.RecordNotFound);
        if (_inventoryAcl.ReturnToInventory(lend) == true)
        {
            lend.IsReturned = true;
            _borrowRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
    #endregion

    #region Delete
    public async Task Delete(int lendId)
    {
        var borrow = await _borrowRepository.GetByIdAsync(lendId);
        await _borrowRepository.DeleteAsync(borrow);
    }

    public void SoftDelete(LoanDto model)
    {
        Borrow borrow = _borrowRepository.GetByIdAsync(model.Id).Result;
        _borrowRepository.SoftDelete(borrow);
    }
    #endregion

    private string GetCurrentOperatorId()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }
}
