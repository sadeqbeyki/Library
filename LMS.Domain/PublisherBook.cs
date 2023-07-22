using LMS.Domain.BookAgg;
using LMS.Domain.PublisherAgg;

namespace LMS.Domain;

public class PublisherBook
{
    public int PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}
