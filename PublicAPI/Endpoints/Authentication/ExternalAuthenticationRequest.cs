using System.ComponentModel.DataAnnotations;
using PublicAPI.Attributes;

namespace PublicAPI.Endpoints.Authentication
{
    public class ExternalAuthenticationRequest
    {
        [Required]
        [CustomAllowedInputValidation(AllowableValues =
            new[] {"google"})]
        public string Provider { get; set; }
        [Required]
        public string ExternalToken { get; set; }
    }
}