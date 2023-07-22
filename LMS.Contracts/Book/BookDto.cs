namespace LMS.Contracts.Book
{
    public class BookDto
    {
        public int PublisherId { get; set; }
        public int AuthorId { get; set; }
        public int TranslatorId { get; set; }
        public int CategoryId { get; set; }

        public string Title { get; set; }
        public string ISBN { get; set; }
        public string Code { get;  set; }
        public string Description { get;  set; }

        //public long CategoryId { get; private set; }
        //public BookCategory Category { get; private set; }
        //public List<AuthorBook> AuthorBooks { get; set; }

        //public List<PublisherBook> PublisherBooks { get; set; }

        //public List<TranslatorBook> TranslatorBooks { get; set; }

        //public List<Reservation> Reservations { get; set; }
    }
}