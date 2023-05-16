using LMS.Domain.BookAgg;
using LMS.Domain.PublisherAgg;

namespace LMS.Domain;

public class PublisherBook
{
    public Guid PublisherId { get; set; }
    public Publisher Publisher { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
