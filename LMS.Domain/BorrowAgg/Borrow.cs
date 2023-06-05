using AppFramework.Domain;
using LI.Domain.UserAgg;
using LMS.Domain.BookAgg;

namespace LMS.Domain.ReservationAgg;

public class Borrow : BaseEntity
{
    public int BookId { get; set; }
    public Book Book { get; set; }
    public int UserId { get; set; }
    public User User { get; set; }
}
