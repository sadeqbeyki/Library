using AppFramework.Domain;
using LMS.Domain.ReservationAgg;

namespace LMS.Domain.UserAgg;

public class User:BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Reservation> Reservations { get; set; }
}
