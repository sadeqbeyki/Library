using AppFramework.Domain;
using BMS.Domain.AuthorAgg;
using BMS.Domain.BookCategoryAgg;
using BMS.Domain.PublisherAgg;
using BMS.Domain.ReservationAgg;
using BMS.Domain.TranslatorAgg;

namespace BMS.Domain.BookAgg
{
    public class Book : BaseEntity
    {
        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Code { get; private set; }
        public string Description { get; private set; }
        public long AuthorId { get; private set; }
        public Author Author { get; set; }
        public long CategoryId { get; private set; }
        public BookCategory Category { get; private set; }
        public long PublisherId { get; private set; }
        public Publisher Publisher { get; set; }
        public long TranslatorId { get; private set; }
        public Translator Translator { get; set; }
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
