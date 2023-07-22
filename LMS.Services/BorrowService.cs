using AppFramework.Application;
using AutoMapper;
using LI.ApplicationContracts.Auth;
using LMS.Contracts.Borrow;
using LMS.Domain.BorrowAgg;
using LMS.Domain.Services;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services;

public class BorrowService : IBorrowService
{
    private readonly IBorrowRepository _borrowRepository;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;
    private readonly ILibraryInventoryAcl _inventoryAcl;
    public BorrowService(IBorrowRepository borrowRepository, IMapper mapper, IAuthHelper authHelper, ILibraryInventoryAcl inventoryAcl)
    {
        _borrowRepository = borrowRepository;
        _mapper = mapper;
        _authHelper = authHelper;
        _inventoryAcl = inventoryAcl;
    }

    public Task Delete(int borrowId)
    {
        throw new NotImplementedException();
    }

    public async Task<List<BorrowDto>> GetAll()
    {
        var loans = await _borrowRepository.GetAll().ToListAsync();
        return _mapper.Map<List<BorrowDto>>(loans);
    }

    public Task<IEnumerable<BorrowDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        throw new NotImplementedException();
    }

    public async Task<BorrowDto> GetBorrowById(int id)
    {
        Borrow loan = await _borrowRepository.GetByIdAsync(id);
        return _mapper.Map<BorrowDto>(loan);
    }

    public Task<IEnumerable<BorrowDto>> GetOverdueBorrows()
    {
        throw new NotImplementedException();
    }
    #region Create
    public async Task<OperationResult> Borrowing(BorrowDto dto)
    {
        OperationResult operationResult = new();
        var currentEmployeeId = _authHelper.CurrentAccountId();

        Borrow borrow = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

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
    public Task<OperationResult> Update(BorrowDto dto)
    {
        throw new NotImplementedException();
    }
}
