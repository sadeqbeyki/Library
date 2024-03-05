using Library.Application.DTOs.BookCategory;
using Microsoft.AspNetCore.Http;

namespace Library.Application.DTOs.Book;

public class UpdateBookModel : BookDto
{
    public int Id { get; set; }
}
