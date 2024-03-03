using Microsoft.AspNetCore.Http;

namespace Library.Application.DTOs.Book;

public class CreateBookModel : BookDto
{
    public IFormFile? Image { get; set; }
}