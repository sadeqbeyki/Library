using AppFramework.Domain;

namespace LibBook.Domain.PublisherAgg
{
    public class Publisher : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        //public List<Book> Books { get; set; }
        public List<BookPublisher> PublisherBooks { get; set; }

    }
}
