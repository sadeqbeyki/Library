namespace BLS.Domain;

public class Loan//reservation
{
    public int Id { get; init; }
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
    public DateTime ReservationDate { get; init; }
}
