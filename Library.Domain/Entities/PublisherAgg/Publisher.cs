using AppFramework.Domain;
using Library.Domain.Entities.Common;

namespace Library.Domain.Entities.PublisherAgg
{
    public class Publisher : BaseEntity<int>
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookPublisher> PublisherBooks { get; set; } = new List<BookPublisher>();

    }
}
