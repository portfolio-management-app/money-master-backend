using Microsoft.AspNetCore.Mvc;

namespace PublicAPI.UserNotification
{
    public class SetReadRequest
    {
        [FromBody] public int id { get; set; }
    }
}