using AppFramework.Domain;
using Library.Domain.Entities.Common;

namespace Library.Domain.Entities.TranslatorAgg;

public class Translator : BaseEntity<int>
{
    public string Name { get; set; }
    public string Description { get; set; }
    public List<BookTranslator> TranslatorBooks { get; set; } = new List<BookTranslator>();
}
