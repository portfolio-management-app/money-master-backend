using System.ComponentModel.DataAnnotations;
using System.Diagnostics.CodeAnalysis;

namespace PublicAPI.Endpoints.User
{
    public class RegisterRequest
    {
        [Required] [EmailAddress] public string Email { get; set; }


        [Required] [MinLength(1)] [NotNull] public string Password { get; set; }
    }
}