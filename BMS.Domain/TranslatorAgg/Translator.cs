using AppFramework.Domain;
using BMS.Domain.BookAgg;

namespace BMS.Domain.TranslatorAgg;

public class Translator:BaseEntity
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<Book> Books { get; set; }
}
