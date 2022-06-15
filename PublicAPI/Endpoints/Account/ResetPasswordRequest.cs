
using System.ComponentModel.DataAnnotations;

namespace PublicAPI.Endpoints.Account
{
    public class ResetPasswordRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string NewPassword { get; set; }

    }
}