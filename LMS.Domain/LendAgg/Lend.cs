using AppFramework.Domain;
using AppFramework.Application;
using LendBook.Domain.LendAgg;

namespace LMS.Domain.LendAgg;

public class Lend : BaseEntity
{
    public Guid BookId { get; set; }
    public string MemberID { get; set; }
    public string EmployeeId { get; set; }
    public string LendDate { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string ReturnEmployeeID { get; set; }
    public string ReturnDate { get; set; }
    public string Description { get; set; }
    public List<LendItem> Items { get; private set; }
    //public bool IsBorrowed { get; private set; }


    public Lend(Guid bookId, string memberID, string employeeId, string lendDate, string idealReturnDate, string returnEmployeeID, string returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        LendDate = lendDate;
        IdealReturnDate = idealReturnDate.ToGeorgianDateTime();
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
    public void Edit(Guid bookId, string memberID, string employeeId, string lendDate, string idealReturnDate, string returnEmployeeID, string returnDate, string description)
    {
        BookId = bookId;
        MemberID = memberID;
        EmployeeId = employeeId;
        LendDate = lendDate;
        IdealReturnDate = idealReturnDate.ToGeorgianDateTime();
        ReturnEmployeeID = returnEmployeeID;
        ReturnDate = returnDate;
        Description = description;
    }
    public void AddItem(LendItem item)
    {
        Items.Add(item);
    }
    //public long CalculateCurrentCount()
    //{
    //    var plus = Operations.Where(x => x.Operation).Sum(x => x.Count);
    //    var minus = Operations.Where(x => !x.Operation).Sum(x => x.Count);
    //    return plus - minus;
    //}

    //public void BookLoan(long count, string operatorId, string description)
    //{
    //    var currentCount = CalculateCurrentCount() - count;
    //    var operation = new LendOperation(false, count, operatorId, currentCount, description, Id);
    //    Operations.Add(operation);
    //    IsBorrowed = currentCount > 0;
    //}
    //public void Return(long count, string operatorId, string description)
    //{
    //    var currentCount = CalculateCurrentCount() + count;
    //    var operation = new LendOperation(true, count, operatorId, currentCount, description, Id);
    //    Operations.Add(operation);
    //    IsBorrowed = currentCount > 0;
    //}
}
