using AppFramework.Domain;
using Library.Domain.BookCategoryAgg;

namespace Library.Domain.BookAgg
{
    public class Book : EntityBase
    {
        public string Code { get; private set; }
        public string Name { get; private set; }
        public string Description { get; private set; }
        public long CategoryId { get; private set; }
        public BookCategory Category{ get; private set; }

        public Book(string code, string name, string description, long categoryId)
        {
            Code = code;
            Name = name;
            Description = description;
            CategoryId = categoryId;
        }
        public void Edit(string code, string name, string description, long categoryId)
        {
            Code = code;
            Name = name;
            Description = description;
            CategoryId = categoryId;
        }
    }
}
