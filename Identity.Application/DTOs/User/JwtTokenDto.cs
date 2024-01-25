using Identity.Domain.Entities.User;

namespace Identity.Application.DTOs.User
{
    public class JwtTokenDto
    {
        public string Token { get; set; }
        public DateTime ExpireOn { get; set; }
        public ApplicationUser User { get; set; }
    }
}
