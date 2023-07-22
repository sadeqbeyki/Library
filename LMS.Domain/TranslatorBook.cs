using LMS.Domain.BookAgg;
using LMS.Domain.TranslatorAgg;

namespace LMS.Domain;

public class TranslatorBook
{
    public int TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}
