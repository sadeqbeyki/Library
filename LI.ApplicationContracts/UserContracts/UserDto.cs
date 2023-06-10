using System.ComponentModel.DataAnnotations;

namespace LI.ApplicationContracts.UserContracts
{
    public class UserDto
    {
        [Required]
        [MaxLength(50)]
        public string FirstName { get; set; }
        [Required]
        [MaxLength(50)]
        public string LastName { get; set; }
        [MaxLength(50)]
        public DateTime? BirthDate { get; set; }
        [Required]
        [MaxLength(50)]
        public string UserName { get; set; }
        [Required]
        [MaxLength(100)]
        public string Email { get; set; }
        [Required]
        [MaxLength(50)]
        public string Password { get; set; }
    }
    public class UpdateUserDto : UserDto
    {
        public string Id { get; set; }

    }
}
