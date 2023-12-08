using humanResourceProject.Domain.Enum;

namespace humanResourceProject.DTO.DTOs
{
    public class LoginDTO
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
