using AppFramework.Domain;
using LMS.Domain.BookAgg;
using LMS.Domain.UserAgg;

namespace LMS.Domain.ReservationAgg;

public class Reservation:BaseEntity
{
    public Book Book { get; set; }
    //public long UserId { get; set; }
    //public User User { get; set; }
    public DateTime ReservationDate { get; init; }
}
