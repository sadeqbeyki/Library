using AppFramework.Domain;
using LMS.Domain.BookAgg;

namespace LMS.Domain.AuthorAgg
{
    public class Author : BaseEntity
    {
        public string Name { get; set; }
        public string Description { get; set; }
        public List<AuthorBook> AuthorBooks { get; set; }
    }
}
