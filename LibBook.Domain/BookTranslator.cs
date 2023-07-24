using LibBook.Domain.BookAgg;
using LibBook.Domain.TranslatorAgg;

namespace LibBook.Domain;

public class BookTranslator
{
    public int TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public int TranslatorBookId { get; set; }
    public Book Book { get; set; }
}
