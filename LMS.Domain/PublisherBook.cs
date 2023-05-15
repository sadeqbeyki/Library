using LMS.Domain.BookAgg;
using LMS.Domain.PublisherAgg;

namespace LMS.Domain;

public class PublisherBook
{
    public long PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public long BookId { get; set; }
    public Book Book { get; set; }
}
