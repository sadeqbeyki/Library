using Library.Domain.Entities.BookAgg;
using Library.Domain.Entities.TranslatorAgg;

namespace Library.Domain.Entities.Common;

public class BookTranslator
{
    public int TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public int TranslatorBookId { get; set; }
    public Book Book { get; set; }
}
