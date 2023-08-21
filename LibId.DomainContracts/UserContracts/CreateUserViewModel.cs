using System.ComponentModel.DataAnnotations;

namespace LibIdentity.DomainContracts.UserContracts;

public class CreateUserViewModel
{
    [Required]
    [MaxLength(50)]
    public string FirstName { get; set; }
    [Required]
    [MaxLength(50)]
    public string LastName { get; set; }
    [MaxLength(50)]
    public string BirthDate { get; set; }
    [Required]
    [MaxLength(50)]
    public string UserName { get; set; }
    //[Required(ErrorMessage = "Email is required")]
    [EmailAddress]
    [MaxLength(100)]
    public string Email { get; set; }

    [Required(ErrorMessage = "Password is required")]
    [MaxLength(50)]
    [DataType(DataType.Password)]
    public string Password { get; set; }

    [Compare("Password", ErrorMessage = "The password and confirmation password do not match.")]
    [DataType(DataType.Password)]
    public string ConfirmPassword { get; set; }

    public string ReturnUrl { get; set; } = "/";

}
