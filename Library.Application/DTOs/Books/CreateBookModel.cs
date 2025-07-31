
namespace Library.Application.DTOs.Books;

public class CreateBookModel : BookDto
{
    public IFormFile? Image { get; set; }
}