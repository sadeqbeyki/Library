using AppFramework.Domain;

namespace LibBook.Domain.AuthorAgg
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<BookAuthor> AuthorBooks { get; set; }
    }
}
