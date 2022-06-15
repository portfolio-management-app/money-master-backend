using System.ComponentModel.DataAnnotations;

namespace PublicAPI.Endpoints.Account
{
    public class UpdatePasswordRequest
    {
        [Required] public string NewPassword { get; set; }
        [Required] public string OldPassword { get; set; }
    }
}