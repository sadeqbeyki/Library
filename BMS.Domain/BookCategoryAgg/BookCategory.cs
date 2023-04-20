using AppFramework.Domain;
using BMS.Domain.BookAgg;

namespace BMS.Domain.BookCategoryAgg
{
    public class BookCategory : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<Book> Books { get; set; }=new List<Book>();

        public BookCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
