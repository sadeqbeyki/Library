using System.ComponentModel.DataAnnotations;

namespace LibBook.DomainContracts.Book;

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

    public List<string> Categories { get; set; }
    public List<string> Authors { get; set; }
    public List<string> Publishers { get; set; }
    public List<string> Translators { get; set; }

    // سایر فیلدها (اگر وجود دارد) را نیز به مدل ویو اضافه کنید

    // در نهایت، می‌توانید فیلدهای مرتبط با کتب را به عنوان پیشنیازها یا فیلدهای مورد نیاز برای ایجاد کتاب اضافه کنید.
}