using AppFramework.Domain;
using AppFramework.Application;

namespace LibBook.Domain.LendAgg;

public class Lend : BaseEntity
{
    public string MemberId { get; set; }
    public string EmployeeId { get; set; }
    public DateTime IdealReturnDate { get; set; }
    public string? ReturnEmployeeId { get; set; }
    public DateTime? ReturnDate { get; set; }
    public string? Description { get; set; }
    public List<LendItem> Items { get; set; }


    public Lend(string memberId, string employeeId, DateTime idealReturnDate, string? returnEmployeeId, DateTime? returnDate, string? description)
    {
        MemberId = memberId;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeId = returnEmployeeId;
        ReturnDate = returnDate;
        Description = description;
        Items = new List<LendItem>();
    }
    public void Edit(string memberId, string employeeId, DateTime idealReturnDate, string? returnEmployeeId, DateTime? returnDate, string? description)
    {
        MemberId = memberId;
        EmployeeId = employeeId;
        IdealReturnDate = idealReturnDate;
        ReturnEmployeeId = returnEmployeeId;
        ReturnDate = returnDate;
        Description = description;
        Items = new List<LendItem>();
    }
    public void AddItem(LendItem item)
    {
        Items.Add(item);
    }
}
