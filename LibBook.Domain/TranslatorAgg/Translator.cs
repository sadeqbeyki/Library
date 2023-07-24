using AppFramework.Domain;

namespace LibBook.Domain.TranslatorAgg;

public class Translator : BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    //public List<Book> Books { get; set; }
    public List<BookTranslator> TranslatorBooks { get; set; }
}
