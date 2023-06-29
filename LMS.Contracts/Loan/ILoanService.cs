namespace LMS.Contracts.Loan
{
    public interface ILoanService
    {
        Task<List<LoanDto>> GetAllLoans();
        Task<LoanDto> GetLoanById(Guid id);
        Task<IEnumerable<LoanDto>> GetLoansByMemberId(int memberId);
        Task<IEnumerable<LoanDto>> GetLoansByEmployeeId(string employeeId);
        Task<IEnumerable<LoanDto>> GetOverdueLoans();
        Task CreateLoan(LoanDto loan);
        Task UpdateLoan(LoanDto loan);
        Task DeleteLoan(int id);
    }

}
