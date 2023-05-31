namespace BLS.Domain;

public class Loan//reservation
{
    public int Id { get; init; }
    public Book Book { get; set; }
    public User User { get; set; }
    public DateTime ReservationDate { get; init; }
}
