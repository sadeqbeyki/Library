using AppFramework.Domain;
using BMS.Domain.ReservationAgg;

namespace BMS.Domain.UserAgg;

public class User:BaseEntity
{
    public string Username { get; set; }
    public string Password { get; set; }
    public List<Reservation> Reservations { get; set; }
}
