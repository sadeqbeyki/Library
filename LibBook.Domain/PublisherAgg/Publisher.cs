using AppFramework.Domain;

namespace LibBook.Domain.PublisherAgg
{
    public class Publisher : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookPublisher> PublisherBooks { get; set; } = new List<BookPublisher>();

    }
}
