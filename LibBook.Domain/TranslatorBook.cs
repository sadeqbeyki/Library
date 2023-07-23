using LibBook.Domain.BookAgg;
using LibBook.Domain.TranslatorAgg;

namespace LibBook.Domain;

public class TranslatorBook
{
    public int TranslatorId { get; set; }
    public Translator Translator { get; set; }

    public int BookId { get; set; }
    public Book Book { get; set; }
}
