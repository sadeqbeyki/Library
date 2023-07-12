using AppFramework.Application;
using AutoMapper;
using LMS.Contracts.Book;
using LMS.Contracts.Loan;
using LMS.Domain.BookAgg;
using LMS.Domain.BookCategoryAgg;
using LMS.Domain.EmployeeAgg;
using LMS.Domain.LoanAgg;
using Microsoft.EntityFrameworkCore;
using System.Collections.Generic;

namespace LMS.Services
{
    public class LoanService : ILoanService
    {
        private readonly ILoanRepository _loanRepository;
        private readonly IMapper _mapper;

        public LoanService(ILoanRepository loanRepository, IMapper mapper)
        {
            _loanRepository = loanRepository;
            _mapper = mapper;
        }

        public async Task<List<LoanDto>> GetAllLoans()
        {
            var loans = await _loanRepository.GetAll().ToListAsync();
            return _mapper.Map<List<LoanDto>>(loans);

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

        public async Task<LoanDto> GetLoanById(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            return _mapper.Map<LoanDto>(loan);
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

        public async Task<IEnumerable<LoanDto>> GetLoansByMemberId(string memberId)
        {
            List<Loan> loans = await _loanRepository.GetAll().ToListAsync();
            var result = loans.Where(l => l.MemberID == memberId).ToList();
            return _mapper.Map<List<LoanDto>>(result);
        }

        public async Task<IEnumerable<LoanDto>> GetLoansByEmployeeId(string employeeId)
        {
            List<Loan> loans = await _loanRepository.GetAll().ToListAsync();
            var result = loans.Where(l => l.EmployeeId == employeeId).ToList();
            return _mapper.Map<List<LoanDto>>(result);
        }

        public async Task<IEnumerable<LoanDto>> GetOverdueLoans()
        {
            List<Loan> loans = await _loanRepository.GetAll().ToListAsync();
            var result = loans.Where(l => l.ReturnDate == null && l.IdealReturnDate < DateTime.Now);
            return _mapper.Map<List<LoanDto>>(result);
        }

        public async Task<LoanDto> CreateLoan(LoanDto dto)
        {
            Loan loan = new(dto.BookId, dto.MemberID, dto.EmployeeId, dto.LoanDate, dto.IdealReturnDate,
                dto.ReturnEmployeeID, dto.ReturnDate, dto.Description);

            var result = await _loanRepository.CreateAsync(loan);
            return _mapper.Map<LoanDto>(result);
        }

        public async Task<OperationResult> UpdateLoan(LoanDto dto)
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

        public async Task DeleteLoan(Guid id)
        {
            var loan = await _loanRepository.GetByIdAsync(id);
            await _loanRepository.DeleteAsync(loan);
        }
    }

}
