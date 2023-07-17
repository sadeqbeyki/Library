using AppFramework.Application;
using AutoMapper;
using BI.ApplicationContracts.Inventory;
using LendBook.ApplicationContract.Lend;
using LendBook.Domain.LendAgg;
using LI.ApplicationContracts.Auth;
using Microsoft.EntityFrameworkCore;

namespace LendBook.ApplicationService;

public class LendService : ILendService
{
    private readonly ILendRepository _lendRepository;
    private readonly IMapper _mapper;
    private readonly IAuthHelper _authHelper;

    public LendService(ILendRepository lendRepository, IMapper mapper, IAuthHelper authHelper)
    {
        _lendRepository = lendRepository;
        _mapper = mapper;
        _authHelper = authHelper;
    }

    public async Task<List<LendDto>> GetAllLends()
    {
        var loans = await _lendRepository.GetAll().ToListAsync();
        return _mapper.Map<List<LendDto>>(loans);

        //    .Select(l => new LoanDto
        //    {
        //        Id = l.Id,
        //        BookId = l.BookId,
        //        Description = l.Description,
        //        EmployeeId = l.EmployeeId,
        //        IdealReturnDate = l.IdealReturnDate,
        //        LoanDate = l.LoanDate,
        //        MemberID = l.MemberID,
        //        ReturnDate = l.ReturnDate,
        //        ReturnEmployeeID = l.ReturnEmployeeID
        //    }).ToListAsync();

        //return result;
    }

    public async Task<LendDto> GetLendById(Guid id)
    {
        var loan = await _lendRepository.GetByIdAsync(id);
        return _mapper.Map<LendDto>(loan);
        //LoanDto result = new()
        //{
        //    Id = loan.Id,
        //    BookId = loan.BookId,
        //    Description = loan.Description,
        //    EmployeeId = loan.EmployeeId,
        //    ReturnEmployeeID = loan.ReturnEmployeeID,
        //    ReturnDate = loan.ReturnDate,
        //    MemberID = loan.MemberID,
        //    LoanDate = loan.LoanDate,
        //    IdealReturnDate = loan.IdealReturnDate,
        //};

        //return result;
    }

    public async Task<IEnumerable<LendDto>> GetLendsByMemberId(string memberId)
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.MemberID == memberId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId)
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.EmployeeId == employeeId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetOverdueLends()
    {
        List<Lend> loans = await _lendRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<LendDto> Create(LendDto dto)
    {
        Lend loan = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LoanDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        var result = await _lendRepository.CreateAsync(loan);
        return _mapper.Map<LendDto>(result);
    }

    public async Task<OperationResult> Update(LendDto dto)
    {
        OperationResult operationResult = new();

        var loan = await _lendRepository.GetByIdAsync(dto.Id);
        if (loan == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        loan.Edit(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LoanDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        await _lendRepository.UpdateAsync(loan);
        return operationResult.Succeeded();
    }

    public async Task Delete(Guid id)
    {
        var loan = await _lendRepository.GetByIdAsync(id);
        await _lendRepository.DeleteAsync(loan);
    }

    public async Task<OperationResult> Return(ReturnBook command)
    {
        var operation = new OperationResult();
        var inventory = await _lendRepository.GetByIdAsync(command.LendId);
        if (inventory == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _authHelper.CurrentAccountId();
        inventory.Return(command.Count, operatorId, command.Description);
        _lendRepository.SaveChanges();
        return operation.Succeeded();
    }

    public async Task<OperationResult> BookLoan(BookLoan command)
    {
        var operation = new OperationResult();
        var lend = await _lendRepository.GetByIdAsync(command.LendId);
        if (lend == null)
            return operation.Failed(ApplicationMessages.RecordNotFound);

        var operatorId = _authHelper.CurrentAccountId();
        lend.BookLoan(command.Count, operatorId, command.Description);
        _lendRepository.SaveChanges();
        return operation.Succeeded();
    }


    public OperationResult BookLoan(List<BookLoan> command)
    {
        var operation = new OperationResult();
        var operatorId = _authHelper.CurrentAccountId();
        foreach (var item in command)
        {
            var lend = _lendRepository.GetBookBy(item.BookId);
            lend.BookLoan(item.Count, operatorId, item.Description);
        }
        _lendRepository.SaveChanges();
        return operation.Succeeded();
    }

}
