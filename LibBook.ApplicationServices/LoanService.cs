using AppFramework.Application;
using AutoMapper;
using LibBook.Domain.BookAgg;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibBook.DomainContracts.Borrow;
using Microsoft.EntityFrameworkCore;

namespace LibBook.ApplicationServices;

public class LoanService : ILoanService
{
    private readonly ILoanRepository _loanRepository;
    private readonly IMapper _mapper;

    private readonly IBookRepository _bookRepository;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    private readonly ILibraryIdentityAcl _IdentityAcl;



    public LoanService(
         ILoanRepository loanRepository,
         IMapper mapper,

         ILibraryInventoryAcl inventoryAcl,
         ILibraryIdentityAcl identityAcl,
         IBookRepository bookRepository)
    {
        _loanRepository = loanRepository;
        _mapper = mapper;

        _inventoryAcl = inventoryAcl;
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
            _IdentityAcl.GetCurrentUserId(),
            model.IdealReturnDate,
            model.ReturnEmployeeID,
            model.ReturnDate,
            model.Description
            );

        var result = await _loanRepository.CreateAsync(borrow);
        return operationResult.Succeeded();
    }

    public async Task<OperationResult> SubmitLend(int lendId)
    {
        OperationResult operationResult = new();

        Borrow lend = await _loanRepository.GetByIdAsync(lendId);
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        //borrow duplicate book check
        var memberDuplicateLoans = await _loanRepository.GetDuplicatedLoans(lend.MemberID, lend.BookId);
        if (memberDuplicateLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.DuplicateLendByMember);

        //Member Overdue Loans
        var memberOverdueLoans = await _loanRepository.GetMemberOverdueLoans(lend.MemberID);
        if (memberOverdueLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.MemberDidntReturnedTheBook);

        if (_inventoryAcl.LoanFromInventory(lend) == true)
        {
            lend.IsApproved = true;
            _loanRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
    #endregion

    #region Read
    public List<LoanDto> GetAll()
    {
        var loans = _loanRepository.GetAll().ToList();
        return _mapper.Map<List<LoanDto>>(loans);
    }
    public List<LoanDto> Search(LoanSearchModel searchModel)
    {
        return _loanRepository.Search(searchModel);
    }

    public async Task<List<LoanDto>> GetPendingLoans()
    {
        var loans = await _loanRepository.GetAll().Where(x => !x.IsDeleted && !x.IsApproved).ToListAsync();

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
        var loans = _loanRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && !x.IsReturned).ToList();
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
        var loans = _loanRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && x.IsReturned).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = Guid.Parse(_IdentityAcl.GetUserName(lend.MemberID).Result),
            EmployeeId = Guid.Parse(_IdentityAcl.GetUserName(lend.EmployeeId).Result),
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnEmployeeID = _IdentityAcl.GetUserName(Guid.Parse(lend.ReturnEmployeeID)).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public List<LoanDto> GetDeletedLoans()
    {
        var loans = _loanRepository.GetAll().Where(x => x.IsDeleted).ToList();
        var result = loans.Select(lend => new LoanDto
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = _bookRepository.GetByIdAsync(lend.BookId).Result.Title,
            MemberId = Guid.Parse(_IdentityAcl.GetUserName(lend.MemberID).Result),
            EmployeeId = Guid.Parse(_IdentityAcl.GetUserName(lend.EmployeeId).Result),
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnEmployeeID = _IdentityAcl.GetUserName(Guid.Parse(lend.ReturnEmployeeID)).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public async Task<List<LoanDto>> GetBorrowsByEmployeeId(Guid employeeId)
    {
        return await _loanRepository.GetBorrowsByEmployeeId(employeeId);
    }

    public async Task<List<LoanDto>> GetBorrowsByMemberId(Guid memberId)
    {
        return await _loanRepository.GetBorrowsByMemberId(memberId);
    }

    public async Task<LoanDto> GetLoanById(int borrowId)
    {
        Borrow borrow = await _loanRepository.GetByIdAsync(borrowId);
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
            ReturnEmployeeID = await _IdentityAcl.GetUserName(Guid.Parse(borrow.ReturnEmployeeID)),
            Description = borrow.Description,
        };
        return dto;
    }

    public async Task<List<LoanDto>> GetOverdueLones()
    {
        var loans = await _loanRepository.GetAll()
            .Where(b => b.IsApproved && b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();
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
            Description = lend.Description ?? string.Empty,
        }).ToList();
        return result;
    }
    #endregion

    #region Update
    public OperationResult Update(LoanDto dto)
    {
        OperationResult operationResult = new();

        var borrow = _loanRepository.GetByIdAsync(dto.Id).Result;
        if (borrow == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        borrow.Edit(
            dto.BookId,
            dto.MemberId,
            dto.EmployeeId,
            dto.IdealReturnDate,
            dto.ReturnEmployeeID,
            dto.ReturnDate,
            dto.Description);

        _loanRepository.UpdateAsync(borrow);
        return operationResult.Succeeded();
    }

    #endregion

    #region Return
    public OperationResult Returning(LoanDto dto)
    {
        OperationResult operationResult = new();

        var lend = _loanRepository.GetByIdAsync(dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        var returnEmployeeID = _IdentityAcl.GetCurrentUserId().ToString();
        lend.Edit(dto.BookId, dto.MemberId, dto.EmployeeId, dto.IdealReturnDate, returnEmployeeID, DateTime.Now, dto.Description);

        if (ReturnLoan(dto.Id).IsSucceeded)
        {
            _loanRepository.UpdateAsync(lend);
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
        Borrow lend = _loanRepository.GetByIdAsync(lendId).Result;
        if (lend == null)
            operationResult.Failed(ApplicationMessages.RecordNotFound);
        if (_inventoryAcl.ReturnToInventory(lend) == true)
        {
            lend.IsReturned = true;
            _loanRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
    #endregion

    #region Delete
    public async Task Delete(int lendId)
    {
        var borrow = await _loanRepository.GetByIdAsync(lendId);
        await _loanRepository.DeleteAsync(borrow);
    }

    public void SoftDelete(LoanDto model)
    {
        Borrow borrow = _loanRepository.GetByIdAsync(model.Id).Result;
        _loanRepository.SoftDelete(borrow);
    }
    #endregion
}
