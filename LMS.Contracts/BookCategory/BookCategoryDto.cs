namespace LMS.Contracts.BookCategory;

public class BookCategoryDto
{
    public Guid Id { get; set; }
    public string Name { get; set; }
    public string Description { get; set; }
    //public List<BookDto> Books { get; set; } = new List<BookDto>();
}
