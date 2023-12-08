namespace humanResourceProject.Models.DTOs
{
    public class LoginDTO
    {

        public string Email { get; set; }

        public string Password { get; set; }

        public string? ReturnUrl { get; set; }
    }
}
