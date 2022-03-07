using AppQuery.Contracts.Book;
using System.Collections.Generic;

namespace AppQuery.Contracts.BookCategory
{
    public class BookCategoryQueryModel
    {
        public long Id { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookQueryModel> Books { get; set; }
    }
}
