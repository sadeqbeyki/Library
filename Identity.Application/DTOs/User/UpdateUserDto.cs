using System.ComponentModel.DataAnnotations;
using System.ComponentModel;

namespace Identity.Application.DTOs.User;

public class UpdateUserDto : CreateUserDto
{
    public string Id { get; set; }

    [Required(ErrorMessage = "RequiredField", AllowEmptyStrings = false)]
    [StringLength(20)]
    [Phone(ErrorMessage = "InvalidPhone")]
    [DisplayName("Phone Number")]
    [DataType(DataType.PhoneNumber)]
    public string PhoneNumber { get; set; } = string.Empty;
}

