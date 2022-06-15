
using System.ComponentModel.DataAnnotations;

namespace PublicAPI.Endpoints.Account
{
    public class ForgetPasswordRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string Lang { get; set; }
    }
}