using AppFramework.Application;
using AutoMapper;
using LibBook.Domain.BorrowAgg;
using LibBook.Domain.Services;
using LibBook.DomainContracts.Borrow;
using LibIdentity.DomainContracts.Auth;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;
using System.Security.Claims;

namespace LibBook.ApplicationServices;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    private readonly IHttpContextAccessor _contextAccessor;
    public BorrowService(IBorrowRepository borrowRepository, IMapper mapper, IAuthHelper authHelper, ILibraryInventoryAcl inventoryAcl, IHttpContextAccessor contextAccessor)
    {
        _borrowRepository = borrowRepository;
        _mapper = mapper;
        _authHelper = authHelper;
        _inventoryAcl = inventoryAcl;
        _contextAccessor = contextAccessor;
    }

    #region Create
    public async Task<OperationResult> Lending(BorrowDto dto)
    {
        OperationResult operationResult = new();

        Borrow borrow = new(
            dto.BookId,
            dto.MemberId,
            GetCurrentOperatorId(),
            dto.IdealReturnDate,
            dto.ReturnEmployeeId,
            dto.ReturnDate,
            dto.Description
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
    public async Task<List<BorrowDto>> GetAll()
    {
        var borrows = await _borrowRepository.GetAll().ToListAsync();
        return _mapper.Map<List<BorrowDto>>(borrows);
    }

    public async Task<List<BorrowDto>> GetAllLoans()
    {
        var result = await _borrowRepository.GetAll()
            .Where(x => x.IsDeleted == false && x.IsApproved == false)
                    .Select(lend => new BorrowDto
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

    public List<BorrowDto> GetAllApprovedLoans()
    {
        var result = _borrowRepository.GetAll()
            .Where(x => x.IsDeleted == false && x.IsApproved == true)
                    .Select(lend => new BorrowDto
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

    public async Task<List<BorrowDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        return await _borrowRepository.GetBorrowsByEmployeeId(employeeId);
    }

    public async Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        return await _borrowRepository.GetBorrowsByMemberId(memberId);
    }

    public async Task<BorrowDto> GetBorrowById(int borrowId)
    {
        Borrow borrow = await _borrowRepository.GetByIdAsync(borrowId);
        BorrowDto dto = new()
        {
            Id = borrow.Id,
            BookId = borrow.BookId,
            MemberId = borrow.MemberID,
            EmployeeId = borrow.EmployeeId,
            BorrowDate = borrow.CreationDate,
            IdealReturnDate = borrow.IdealReturnDate,
            ReturnDate = borrow.ReturnDate,
            ReturnEmployeeId = borrow.ReturnEmployeeID,
            Description = borrow.Description,
        };
        return dto;
    }

    public async Task<List<BorrowDto>> GetOverdueBorrows()
    {
        return await _borrowRepository.GetOverdueBorrows();
    }
    #endregion

    #region Update
    public OperationResult Update(BorrowDto dto)
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
    public OperationResult Returning(BorrowDto dto)
    {
        OperationResult operationResult = new();

        var borrow = _borrowRepository.GetByIdAsync(dto.Id).Result;
        if (borrow == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        borrow.Edit(dto.BookId, dto.MemberId, dto.EmployeeId, dto.IdealReturnDate, GetCurrentOperatorId(), DateTime.Now, dto.Description);


        if (ReturnLoan(dto.Id).IsSucceeded)
        {
            _borrowRepository.UpdateAsync(borrow);
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

    public async Task SoftDeleteAsync(BorrowDto model)
    {
        Borrow borrow = await _borrowRepository.GetByIdAsync(model.Id);
        await _borrowRepository.SoftDeleteAsync(borrow);
    }
    #endregion

    private string GetCurrentOperatorId()
    {
        return _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
    }


}
