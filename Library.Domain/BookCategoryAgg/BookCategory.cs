using AppFramework.Domain;
using Library.Domain.BookAgg;
using System.Collections.Generic;

namespace Library.Domain.BookCategoryAgg
{
    public class BookCategory : EntityBase
    {
        public string Name { get; private set; }
        public string Description { get; private set; }
        public List<Book> Books { get; set; }

        public BookCategory()
        {
            Books = new List<Book>();
        }

        public BookCategory(string name, string description)
        {
            Name = name;
            Description = description;
        }
        public void Edit(string name, string description)
        {
            Name = name;
            Description = description;
        }
    }
}
