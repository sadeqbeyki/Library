using AppFramework.Domain;
using Library.Application.DTOs.Lend;
using Library.Domain.Entities.LendAgg;

namespace Library.Application.Interfaces;

public interface ILendRepository : IRepository<Lend, int>
{
    Task<List<LendDto>> GetLoansByMemberId(Guid memberId);
    Task<List<LendDto>> GetMemberOverdueLoans(Guid memberId);
    Task<List<LendDto>> GetDuplicatedLoans(Guid memberId, int bookId);
    Task<List<LendDto>> GetLoansByEmployeeId(Guid EmployeeId);
    List<LendDto> Search(LendSearchModel searchModel);
    Task<string> GetReturnEmployeeName(Guid? id);
}
