using LMS.Contracts.Book;
using LMS.Contracts.Loan;
using LMS.Domain.BookAgg;
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
            return null;
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByEmployeeId(string employeeId)
        {
            return null;
        }

        public async Task<IEnumerable<LoanDto>> GetOverdueLoans()
        {
            // Implement the logic to retrieve overdue loans
            // Example: return await _loanRepository.FindAsync(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
            // You can customize the condition based on your business logic
            return null;
        }

        public async Task<LoanDto> CreateLoan(LoanDto dto)
        {
            var loan = new Loan
            {
                BookId = dto.BookId,
                MemberID = dto.MemberID,
                EmployeeId = dto.EmployeeId,
                LoanDate = dto.LoanDate,
                IdealReturnDate = dto.IdealReturnDate,
                ReturnEmployeeID = dto.ReturnEmployeeID,
                ReturnDate = dto.ReturnDate,
                Description = dto.Description,

            };
            var newLoan = await _loanRepository.CreateAsync(loan);

            var result = new LoanDto
            {
                BookId = newLoan.BookId,
                MemberID = newLoan.MemberID,
                EmployeeId = newLoan.EmployeeId,
                LoanDate = newLoan.LoanDate,
                IdealReturnDate = newLoan.IdealReturnDate,
                ReturnEmployeeID = newLoan.ReturnEmployeeID,
                ReturnDate = newLoan.ReturnDate,
                Description = newLoan.Description,
            };

            return result;
        }

        public async Task<LoanDto> UpdateLoan(LoanDto dto)
        {
            var existLoan = await _loanRepository.GetByIdAsync(dto.Id);
            if (existLoan == null)
                return dto;
            
            existLoan.BookId = dto.BookId;  
            existLoan.MemberID = dto.MemberID;
            existLoan.EmployeeId = dto.EmployeeId;
            existLoan.LoanDate= dto.LoanDate;
            existLoan.IdealReturnDate = dto.IdealReturnDate;
            existLoan.ReturnEmployeeID = dto.ReturnEmployeeID;
            existLoan.ReturnDate = dto.ReturnDate;
            existLoan.Description = dto.Description;    

            await _loanRepository.UpdateAsync(existLoan);
            return dto;
        }

        public async Task DeleteLoan(int id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);

            await _loanRepository.DeleteAsync(loan);
            return dto;
        }
    }

}
