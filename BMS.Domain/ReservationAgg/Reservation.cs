using AppFramework.Domain;
using BMS.Domain.BookAgg;
using BMS.Domain.UserAgg;

namespace BMS.Domain.ReservationAgg;

public class Reservation:BaseEntity
{
    public Book Book { get; set; }
    public User User { get; set; }
    public DateTime ReservationDate { get; init; }
}
