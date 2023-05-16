using AppFramework.Domain;
using LMS.Domain.BookAgg;

namespace LMS.Domain.TranslatorAgg;

public class Translator:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    //public List<Book> Books { get; set; }
    public List<TranslatorBook> TranslatorBooks { get; set; }
}
