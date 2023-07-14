using AppFramework.Application;
using AutoMapper;
using LMS.Contracts.Loan;
using LMS.Domain.LendAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services;

public class LoanService : ILendService
{
    private readonly ILendRepository _loanRepository;
    private readonly IMapper _mapper;

    public LoanService(ILendRepository loanRepository, IMapper mapper)
    {
        _loanRepository = loanRepository;
        _mapper = mapper;
    }

    public async Task<List<LendDto>> GetAllLends()
    {
        var loans = await _loanRepository.GetAll().ToListAsync();
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
        var loan = await _loanRepository.GetByIdAsync(id);
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
        List<Lend> loans = await _loanRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.MemberID == memberId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetLendsByEmployeeId(string employeeId)
    {
        List<Lend> loans = await _loanRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.EmployeeId == employeeId).ToList();
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<IEnumerable<LendDto>> GetOverdueLends()
    {
        List<Lend> loans = await _loanRepository.GetAll().ToListAsync();
        var result = loans.Where(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
        return _mapper.Map<List<LendDto>>(result);
    }

    public async Task<LendDto> Create(LendDto dto)
    {
        Lend loan = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LoanDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        var result = await _loanRepository.CreateAsync(loan);
        return _mapper.Map<LendDto>(result);
    }

    public async Task<OperationResult> Update(LendDto dto)
    {
        OperationResult operationResult = new();

        var loan = await _loanRepository.GetByIdAsync(dto.Id);
        if (loan == null)
            return operationResult.Failed(ApplicationMessages.RecordNotFound);

        loan.Edit(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LoanDate, dto.IdealReturnDate,
            dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

        await _loanRepository.UpdateAsync(loan);
        return operationResult.Succeeded();
    }

    public async Task Delete(Guid id)
    {
        var loan = await _loanRepository.GetByIdAsync(id);
        await _loanRepository.DeleteAsync(loan);
    }
}
