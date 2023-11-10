using AppFramework.Application;
using AutoMapper;
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

    private readonly ILibraryInventoryAcl _inventoryAcl;
    private readonly ILibraryIdentityAcl _IdentityAcl;
    private readonly IHttpContextAccessor _contextAccessor;


    public LoanService(ILoanRepository borrowRepository, IMapper mapper, ILibraryInventoryAcl inventoryAcl,
        IHttpContextAccessor contextAccessor, ILibraryIdentityAcl identityAcl)
    {
        _borrowRepository = borrowRepository;
        _mapper = mapper;

        _inventoryAcl = inventoryAcl;
        _contextAccessor = contextAccessor;
        _IdentityAcl = identityAcl;
    }

    #region Create
    public async Task<OperationResult> Lending(LoanDto model)
    {
        OperationResult operationResult = new();
        if (model.BookId == 0 || model.MemberId == null)
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
    public async Task<List<LoanDto>> GetAll()
    {
        var borrows = await _borrowRepository.GetAll().ToListAsync();
        return _mapper.Map<List<LoanDto>>(borrows);
    }

    public async Task<List<LoanDto>> GetAllLoans()
    {
        var result = await _borrowRepository.GetAll()
            .Where(x => !x.IsDeleted && x.IsApproved == false)
                    .Select(lend => new LoanDto
                    {
                        Id = lend.Id,
                        BookId = lend.BookId,
                        MemberId = lend.MemberID,
                        EmployeeId = lend.EmployeeId,
                        BorrowDate = lend.CreationDate,
                        IdealReturnDate = lend.IdealReturnDate,
                        ReturnEmployeeId = lend.ReturnEmployeeID,
                        ReturnDate = lend.ReturnDate,
                        Description = lend.Description,
                    }).ToListAsync();
        return result;
    }

    public List<LoanDto> GetApprovedLoans()
    {
        var result = _borrowRepository.GetAll()
            .Where(x => !x.IsDeleted && x.IsApproved && !x.IsReturned)
                    .Select(lend => new LoanDto
                    {
                        Id = lend.Id,
                        BookId = lend.BookId,
                        MemberId = lend.MemberID,
                        EmployeeId = lend.EmployeeId,
                        BorrowDate = lend.CreationDate,
                        IdealReturnDate = lend.IdealReturnDate,
                        ReturnEmployeeId = lend.ReturnEmployeeID,
                        ReturnDate = lend.ReturnDate,
                        Description = lend.Description,
                    }).ToList();
        return result;
    }

    public List<LoanDto> GetReturnedLoans()
    {
        var result = _borrowRepository.GetAll()
            .Where(x => !x.IsDeleted && x.IsApproved && x.IsReturned)
                    .Select(lend => new LoanDto
                    {
                        Id = lend.Id,
                        BookId = lend.BookId,
                        MemberId = lend.MemberID,
                        EmployeeId = lend.EmployeeId,
                        BorrowDate = lend.CreationDate,
                        IdealReturnDate = lend.IdealReturnDate,
                        ReturnEmployeeId = lend.ReturnEmployeeID,
                        ReturnDate = lend.ReturnDate,
                        Description = lend.Description,
                    }).ToList();
        return result;
    }

    public List<LoanDto> GetDeletedLoans()
    {
        var result = _borrowRepository.GetAll()
            .Where(x => x.IsDeleted)
                    .Select(lend => new LoanDto
                    {
                        Id = lend.Id,
                        BookId = lend.BookId,
                        MemberId = lend.MemberID,
                        EmployeeId = lend.EmployeeId,
                        BorrowDate = lend.CreationDate,
                        IdealReturnDate = lend.IdealReturnDate,
                        ReturnEmployeeId = lend.ReturnEmployeeID,
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
        LoanDto dto = new()
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            MemberId = await _IdentityAcl.GetUserName(borrow.MemberID),
            EmployeeId = await _IdentityAcl.GetUserName(borrow.EmployeeId),
            BorrowDate = borrow.CreationDate,
            IdealReturnDate = borrow.IdealReturnDate,
            ReturnDate = borrow.ReturnDate,
            ReturnEmployeeId = await _IdentityAcl.GetUserName(borrow.ReturnEmployeeID),
            Description = borrow.Description,
        };
        return dto;
    }

    public async Task<List<LoanDto>> GetOverdueLones()
    {
        return await _borrowRepository.GetOverdueLones();
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

        lend.Edit(
            dto.BookId,
            dto.MemberId,
            dto.EmployeeId,
            dto.IdealReturnDate,
            GetCurrentOperatorId(),
            DateTime.Now,
            dto.Description
            );


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
