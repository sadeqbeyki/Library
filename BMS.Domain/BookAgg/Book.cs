

using AppFramework.Domain;
using BMS.Domain.BookCategoryAgg;
using BMS.Domain.PublisherAgg;
using BMS.Domain.ReservationAgg;

namespace BMS.Domain.BookAgg
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string Author { get; set; }
        public string ISBN { get; set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public long CategoryId { get; private set; }
        public BookCategory Category { get; private set; }
        public Publisher Publisher { get; set; }
        public List<Reservation> Reservations { get; set; }

        public Book(string code, string title, string description, long categoryId)
        {
            Code = code;
            Title = title;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
