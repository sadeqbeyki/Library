using System.ComponentModel.DataAnnotations;

namespace LibIdentity.DomainContracts.UserContracts
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
        public string BirthDate { get; set; }
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
        public int Id { get; set; }
    }
}
