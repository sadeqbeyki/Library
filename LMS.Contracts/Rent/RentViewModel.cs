namespace LMS.Contracts.Rent;

public class RentViewModel
{
    public Guid Id { get; set; }
    public string AccountId { get; set; }
    public string AccountFullName { get; set; }
    public int PaymentMethodId { get; set; }
    public string PaymentMethod { get; set; }
    public double TotalAmount { get; set; }
    public double DiscountAmount { get; set; }
    public double PayAmount { get; set; }
    public bool IsPaid { get; set; }
    public bool IsCanceled { get; set; }
    public string IssueTrackingNo { get; set; }
    public long RefId { get; set; }
    public string CreationDate { get; set; }
}