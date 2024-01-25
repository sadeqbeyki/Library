using Microsoft.AspNetCore.Mvc;
using System.ComponentModel.DataAnnotations;

namespace Identity.Application.DTOs.User
{
    /// <summary>
    /// Model that used for sending Login details to API
    /// </summary>
    public class LoginUserDto
    {
        //[Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
        //[EmailAddress(ErrorMessage = "InvalidMailId")]
        //[DataType(DataType.EmailAddress)]
        //[Display(Name = "Email Address")]
        //public string Email { get; set; }

        [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
        [Display(Name = "User Name / Email ")]
        public string Username { get; set; }

        [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
        [DataType(DataType.Password)]
        [UIHint("Password")]
        public string Password { get; set; }

        [BindProperty(Name = "ReturnUrl", SupportsGet = true)]
        public string ReturnUrl { get; set; } = "/";

        [Display(Name = "Remember me?")]
        public bool RememberMe { get; set; }
    }
}
