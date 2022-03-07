using System.Collections.Generic;

namespace Library.Application.Contracts.Book
{
    public class CreateBook
    {
        public string Code { get; set; }
        public string Name { get; set; }
        public string Description { get; set; }
        public long CategoryId {get; set; }
        public List<BookCategoryViewModel> Categories { get; set; }
    }
}
