using System.Collections.Generic;
using System.Threading.Tasks;

namespace ApplicationCore.Interfaces
{
    public interface IFireBaseAdminMessagingService
    {
        Task SendSingleNotification(string fcmCode, Dictionary<string, string> data);
        Task SendMultiNotification(List<string> registerTokens, Dictionary<string, string> data);
    }
}