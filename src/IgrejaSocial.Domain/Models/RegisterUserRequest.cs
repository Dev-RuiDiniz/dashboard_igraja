namespace IgrejaSocial.Domain.Models
{
    public class RegisterUserRequest
    {
        public string Email { get; set; } = string.Empty;
        public string Password { get; set; } = string.Empty;
        public string[] Roles { get; set; } = System.Array.Empty<string>();
    }
}
