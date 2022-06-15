using System.ComponentModel.DataAnnotations;

namespace PublicAPI.Endpoints.Account
{
    public class VerifyOTPRequest
    {
        [Required] public string Email { get; set; }
        [Required] public string OtpCode { get; set; }
    }
}