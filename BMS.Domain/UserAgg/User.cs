using AppFramework.Domain;
using BMS.Domain.ReservationAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.UserAgg
{
    public class User:BaseEntity
    {
        public string Username { get; set; }
        public string Password { get; set; }
        public List<Reservation> Reservations { get; set; }
    }
}
