using AppFramework.Domain;
using BMS.Domain.BookAgg;
using BMS.Domain.UserAgg;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace BMS.Domain.ReservationAgg
{
    public class Reservation:BaseEntity
    {
        public Book Book { get; set; }
        public User User { get; set; }
        public DateTime ReservationDate { get; init; }
    }
}
