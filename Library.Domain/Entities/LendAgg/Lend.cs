﻿using AppFramework.Domain;
using System.Diagnostics.CodeAnalysis;

namespace Library.Domain.Entities.LendAgg;

public class Lend : BaseEntity<int>
{
    public int BookId { get; set; }
    public Guid MemberID { get; set; }
    public Guid EmployeeId { get; set; }
    public DateTime IdealReturnDate { get; set; }

    public Guid? ReturnEmployeeID { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? Description { get; set; }

    public bool IsApproved { get; set; }
    public bool IsReturned { get; set; }


    public Lend(int bookId, Guid memberID, Guid employeeId, DateTime idealReturnDate, Guid? returnEmployeeID, DateTime? returnDate, string? description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
        IsApproved = false;
        IsReturned = false;
    }
    public void Edit(int bookId, Guid memberID, Guid employeeId, DateTime idealReturnDate, Guid? returnEmployeeID, DateTime? returnDate, string? description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
}
