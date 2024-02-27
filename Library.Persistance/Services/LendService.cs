using AppFramework.Application;
using AutoMapper;
using Library.ACL.Identity;
using Library.ACL.Inventory;
using Library.Application.Contracts;
using Library.Application.DTOs.Lend;
using Library.Application.Interfaces;
using Library.Domain.Entities.LendAgg;
using Microsoft.EntityFrameworkCore;

namespace Library.Persistance.Services;

public class LendService : ILendService
{
    private readonly ILendRepository _loanRepository;
    private readonly IMapper _mapper;

    private readonly IBookRepository _bookRepository;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    private readonly ILibraryIdentityAcl _IdentityAcl;



    public LendService(
         ILendRepository loanRepository,
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
    public async Task<OperationResult> Lending(LendDto model)
    {
        OperationResult operationResult = new();
        if (model.BookId <= 0 || model.MemberId == null)
            return operationResult.Failed(ApplicationMessages.ModelIsNull);

        Lend lend = new(
            model.BookId,
            model.MemberId,
            _IdentityAcl.GetCurrentUserId(),
            model.IdealReturnDate,
            model.ReturnEmployeeID,
            model.ReturnDate,
            model.Description
            );

        var result = await _loanRepository.CreateAsync(lend);
        return operationResult.Succeeded();
    }

    public async Task<OperationResult> SubmitLend(int lendId)
    {
        OperationResult operationResult = new();

        Lend lend = await _loanRepository.GetByIdAsync(lendId);
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        //lend duplicate book check
        var memberDuplicateLoans = await _loanRepository.GetDuplicatedLoans(lend.MemberID, lend.BookId);
        if (memberDuplicateLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.DuplicateLendByMember);

        //Member Overdue Loans
        var memberOverdueLoans = await _loanRepository.GetMemberOverdueLoans(lend.MemberID);
        if (memberOverdueLoans.Count > 0)
            return operationResult.Failed(ApplicationMessages.MemberDidntReturnedTheBook);

        if (_inventoryAcl.BorrowFromInventory(lend) == true)
        {
            lend.IsApproved = true;
            _loanRepository.SaveChanges();
            return operationResult.Succeeded();
        }
        return operationResult.Failed();
    }
    #endregion

    #region Read
    public List<LendDto> GetAll()
    {
        var loans = _loanRepository.GetAll().ToList();
        return _mapper.Map<List<LendDto>>(loans);
    }
    public List<LendDto> Search(LendSearchModel searchModel)
    {
        return _loanRepository.Search(searchModel);
    }

    public async Task<List<LendDto>> GetPendingLoans()
    {
        var loans = await _loanRepository.GetAll().Where(x => !x.IsDeleted && !x.IsApproved).ToListAsync();

        var result = loans.Select(lend => new LendDto
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

    public List<LendDto> GetApprovedLoans()
    {
        var loans = _loanRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && !x.IsReturned).ToList();
        var result = loans.Select(lend => new LendDto
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

    public List<LendDto> GetReturnedLoans()
    {
        var loans = _loanRepository.GetAll().Where(x => !x.IsDeleted && x.IsApproved && x.IsReturned).ToList();
        var result = loans.Select(lend => new LendDto
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
            ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public List<LendDto> GetDeletedLoans()
    {
        var loans = _loanRepository.GetAll().Where(x => x.IsDeleted).ToList();
        var result = loans.Select(lend => new LendDto
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
            ReturnEmployeeID = lend.ReturnEmployeeID,
            ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            ReturnDate = lend.ReturnDate,
            Description = lend.Description,
        }).ToList();
        return result;
    }

    public async Task<List<LendDto>> GetLoansByEmployeeId(Guid employeeId)
    {
        return await _loanRepository.GetLoansByEmployeeId(employeeId);
    }

    public async Task<List<LendDto>> GetLoansByMemberId(Guid memberId)
    {
        return await _loanRepository.GetLoansByMemberId(memberId);
    }

    public async Task<LendDto> GetLendById(int borrowId)
    {
        Lend lend = await _loanRepository.GetByIdAsync(borrowId);
        var book = await _bookRepository.GetByIdAsync(lend.BookId);

        LendDto dto = new()
        {
            Id = lend.Id,
            BookId = lend.BookId,
            BookTitle = book.Title,
            MemberId = lend.MemberID,
            MemberName = await _IdentityAcl.GetUserName(lend.MemberID),
            EmployeeId = lend.EmployeeId,
            EmployeeName = await _IdentityAcl.GetUserName(lend.EmployeeId),
            CreationDate = lend.CreationDate,
            IdealReturnDate = lend.IdealReturnDate,
            ReturnDate = lend.ReturnDate,
            ReturnEmployeeName = _loanRepository.GetReturnEmployeeName(lend.ReturnEmployeeID).Result,
            Description = lend.Description,
        };
        return dto;
    }

    public async Task<List<LendDto>> GetOverdueLones()
    {
        var loans = await _loanRepository.GetAll()
            .Where(b => b.IsApproved && b.ReturnDate == null && b.IdealReturnDate < DateTime.Now).ToListAsync();
        var result = loans.Select(lend => new LendDto
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
    public OperationResult Update(LendDto dto)
    {
        OperationResult operationResult = new();

        var lend = _loanRepository.GetByIdAsync(dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        lend.Edit(
            dto.BookId,
            dto.MemberId,
            dto.EmployeeId,
            dto.IdealReturnDate,
            dto.ReturnEmployeeID,
            dto.ReturnDate,
            dto.Description);

        _loanRepository.UpdateAsync(lend);
        return operationResult.Succeeded();
    }

    #endregion

    #region Return
    public OperationResult Returning(LendDto dto)
    {
        OperationResult operationResult = new();

        var lend = _loanRepository.GetByIdAsync(dto.Id).Result;
        if (lend == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);
        var returnEmployeeID = _IdentityAcl.GetCurrentUserId();
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
        Lend lend = _loanRepository.GetByIdAsync(lendId).Result;
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
        var lend = await _loanRepository.GetByIdAsync(lendId);
        await _loanRepository.DeleteAsync(lend);
    }

    public void SoftDelete(LendDto model)
    {
        Lend lend = _loanRepository.GetByIdAsync(model.Id).Result;
        _loanRepository.SoftDelete(lend);
    }
    #endregion
}
