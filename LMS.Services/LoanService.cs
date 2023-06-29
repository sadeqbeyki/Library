using LMS.Contracts.Loan;
using LMS.Domain.LoanAgg;
using Microsoft.EntityFrameworkCore;

namespace LMS.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;

        public LoanService(ILoanRepository loanRepository)
        {
            _loanRepository = loanRepository;
        }

        public async Task<List<LoanDto>> GetAllLoans()
        {
            var result = await _loanRepository.GetAll()
                .Select(l => new LoanDto
                {
                    Id = l.Id,
                    BookId = l.BookId,
                    Description = l.Description,
                    EmployeeId = l.EmployeeId,
                    IdealReturnDate = l.IdealReturnDate,
                    LoanDate = l.LoanDate,
                    MemberID = l.MemberID,
                    ReturnDate = l.ReturnDate,
                    ReturnEmployeeID = l.ReturnEmployeeID
                }).ToListAsync();

            return result;
        }

        public async Task<LoanDto> GetLoanById(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            LoanDto result = new()
            {
                Id = loan.Id,
                BookId = loan.BookId,
                Description = loan.Description,
                EmployeeId = loan.EmployeeId,
                ReturnEmployeeID = loan.ReturnEmployeeID,
                ReturnDate = loan.ReturnDate,
                MemberID = loan.MemberID,
                LoanDate = loan.LoanDate,
                IdealReturnDate = loan.IdealReturnDate,
            };
            return result;
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByMemberId(int memberId)
        {
            return await _loanRepository.GetByIdAsync(l => l.MemberID == memberId);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByEmployeeId(string employeeId)
        {
            return await _loanRepository.FindAsync(l => l.EmployeeId == employeeId);
        }

        public async Task<IEnumerable<LoanDto>> GetOverdueLoans()
        {
            // Implement the logic to retrieve overdue loans
            // Example: return await _loanRepository.FindAsync(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
            // You can customize the condition based on your business logic
        }

        public async Task CreateLoan(LoanDto loan)
        {
            await _loanRepository.AddAsync(loan);
        }

        public async Task UpdateLoan(LoanDto loan)
        {
            _loanRepository.Update(loan);
            await _loanRepository.SaveChangesAsync();
        }

        public async Task DeleteLoan(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            if (loan != null)
            {
                _loanRepository.Remove(loan);
                await _loanRepository.SaveChangesAsync();
            }
        }
    }

}
