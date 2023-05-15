using LMS.Domain.BookAgg;
using LMS.Domain.TranslatorAgg;

namespace LMS.Domain;

public class TranslatorBook
{
    public long TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public long BookId { get; set; }
    public Book Book { get; set; }
}
