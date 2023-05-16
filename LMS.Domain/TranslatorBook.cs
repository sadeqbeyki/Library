using LMS.Domain.BookAgg;
using LMS.Domain.TranslatorAgg;

namespace LMS.Domain;

public class TranslatorBook
{
    public Guid TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public Guid BookId { get; set; }
    public Book Book { get; set; }
}
