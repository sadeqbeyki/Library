using LibBook.DomainContracts.BookCategory;
using System.ComponentModel.DataAnnotations;

namespace Library.EndPoint.Areas.adminPanel.Models;

public class BookCreateViewModel
{
    [Required(ErrorMessage = "فیلد عنوان کتاب اجباری است.")]
    public string Title { get; set; }
    [Required(ErrorMessage = "فیلد شابک اجباری است.")]
    public string ISBN { get; set; }
    [Required(ErrorMessage = "فیلد کد اجباری است.")]
    public string Code { get; set; }
    public string Description { get; set; }

    [Required(ErrorMessage = "فیلد دسته بندی اجباری است.")]
    public int CategoryId { get; set; }
    //public string Category { get; set; }

    public List<BookCategoryDto> BookCategories { get; set; }
    //public List<string> Categories { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Publishers { get; set; }
    public List<string> Translators { get; set; }
}