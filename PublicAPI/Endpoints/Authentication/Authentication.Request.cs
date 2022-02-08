using System.ComponentModel.DataAnnotations;

namespace PublicAPI.Endpoints.Authentication
{
    public class AuthenticationRequest
    {
        [Required]
        public string Email { get; set; }
        [Required]
        public string Password { get; set; }
    }
}