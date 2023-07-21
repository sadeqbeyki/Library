using AppFramework.Application;
using AutoMapper;
using LI.ApplicationContracts.Auth;
using LMS.Contracts.Borrow;
using LMS.Domain.Borrow;
using LMS.Domain.Services;

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

    public Task Delete(Guid borrowId)
    {
        throw new NotImplementedException();
    }

    public Task<List<BorrowDto>> GetAll()
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BorrowDto>> GetBorrowsByEmployeeId(string employeeId)
    {
        throw new NotImplementedException();
    }

    public Task<IEnumerable<BorrowDto>> GetBorrowsByMemberId(string memberId)
    {
        throw new NotImplementedException();
    }

    public Task<BorrowDto> GetLendById(Guid id)
    {
        throw new NotImplementedException();
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

        Borrow borrow = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LendDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        var result = await _borrowRepository.CreateAsync(borrow);
        return operationResult.Succeeded();
    }

    public Task<OperationResult> BorrowingRegistration(Guid borrowId)
    {
        throw new NotImplementedException();
    }
    #endregion
    public Task<OperationResult> Update(BorrowDto dto)
    {
        throw new NotImplementedException();
    }
}
