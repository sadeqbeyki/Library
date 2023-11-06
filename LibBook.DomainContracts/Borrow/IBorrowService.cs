﻿using AppFramework.Application;

namespace LibBook.DomainContracts.Borrow;

public interface IBorrowService
{
    Task<List<BorrowDto>> GetAll();
    Task<List<BorrowDto>> GetAllLoans();
    List<BorrowDto> GetApprovedLoans();
    List<BorrowDto> GetReturnedLoans();
    List<BorrowDto> GetDeletedLoans();

    Task<BorrowDto> GetBorrowById(int borrowId);
    Task<List<BorrowDto>> GetBorrowsByMemberId(string memberId);
    Task<List<BorrowDto>> GetBorrowsByEmployeeId(string EmployeeId);

    Task<List<BorrowDto>> GetOverdueLones();

    OperationResult Update(BorrowDto dto);
    Task Delete(int borrowId);
    void SoftDelete(BorrowDto entity);

    Task<OperationResult> Lending(BorrowDto dto);
    Task<OperationResult> SubmitLend(int borrowId);

    OperationResult Returning(BorrowDto dto);
}
