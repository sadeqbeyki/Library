using AppFramework.Domain;
using LMS.Domain.ReservationAgg;

namespace LMS.Domain.UserAgg;

public class Borrower : BaseEntity
{
    public string Username { get; set; }
    public List<Borrow> Reservations { get; set; }
}
