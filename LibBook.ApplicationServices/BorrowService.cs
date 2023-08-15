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
    public async Task<OperationResult> Borrowing(BorrowDto dto)
    {
        OperationResult operationResult = new();
        var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);

        Borrow borrow = new(dto.BookId, dto.MemberId, operatorId, dto.IdealReturnDate,
            dto.ReturnEmployeeId, dto.ReturnDate, dto.Description);

        var result = await _borrowRepository.CreateAsync(borrow);
        return operationResult.Succeeded();
    }

    public async Task<OperationResult> BorrowingRegistration(int borrowId)
    {
        OperationResult operationResult = new();
        Borrow borrow = await _borrowRepository.GetByIdAsync(borrowId);
        if (borrow == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        _inventoryAcl.BorrowFromInventory(borrow);
        _borrowRepository.SaveChanges();
        return operationResult.Succeeded();
    }
    #endregion

    #region Read
    public async Task<List<BorrowDto>> GetAll()
    {
        var borrows = await _borrowRepository.GetAll().ToListAsync();
        return _mapper.Map<List<BorrowDto>>(borrows);
    }

    public async Task<List<BorrowDto>> GetAllBorrows()
    {
        var result = await _borrowRepository.GetAll()
                    .Select(borrow => new BorrowDto
                    {
                        Id = borrow.Id,
                        BookId = borrow.Id,
                        MemberId = borrow.MemberID,
                        EmployeeId = borrow.EmployeeId,
                        BorrowDate = borrow.CreationDate,
                        IdealReturnDate = borrow.IdealReturnDate,
                        ReturnEmployeeId = borrow.ReturnEmployeeID,
                        ReturnDate = borrow.ReturnDate,
                        Description = borrow.Description,
                    }).ToListAsync();
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
    public async Task<OperationResult> Update(BorrowDto dto)
    {
        OperationResult operationResult = new();

        var borrow = await _borrowRepository.GetByIdAsync(dto.Id);
        if (borrow == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _contextAccessor.HttpContext?.User.FindFirstValue(ClaimTypes.NameIdentifier);
        borrow.Edit(dto.BookId, dto.MemberId, dto.EmployeeId, dto.IdealReturnDate,
            dto.ReturnEmployeeId = operatorId, dto.ReturnDate = DateTime.Now, dto.Description);

        await _borrowRepository.UpdateAsync(borrow);
        return operationResult.Succeeded();
    }

    public async Task<OperationResult> ReturnBorrow(int borrowId)
    {
        OperationResult operationResult = new();
        Borrow borrow = await _borrowRepository.GetByIdAsync(borrowId);
        if (borrow == null)
            operationResult.Failed(ApplicationMessages.RecordNotFound);
        _inventoryAcl.ReturnToInventory(borrow);
        _borrowRepository.SaveChanges();
        return operationResult.Succeeded();
    }
    #endregion

    #region Delete
    public async Task Delete(int borrowId)
    {
        var borrow = await _borrowRepository.GetByIdAsync(borrowId);
        await _borrowRepository.DeleteAsync(borrow);
    }

    #endregion

}
