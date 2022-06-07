using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.AssetAggregate.CashAggregate.DTOs;
using ApplicationCore.NotificationAggregate.DTOs;
using ApplicationCore.Entity;

namespace ApplicationCore.NotificationAggregate
{
    public interface INotificationService
    {
        public Notification RegisterPriceNotification(int userId, int portfolioId, NotificationDto dto, bool isHigh);

        public Notification EditNotification(int id, EditNotificationDto dto);
        public List<Notification> GetActiveHighNotifications(string assetType);
        public List<Notification> GetActiveLowNotifications(string assetType);

        public void TurnOffHighNotificationById(int id);


        public void TurnOffLowNotificationById(int id);
        public Notification GetNotificationByAssetIdAndAssetType(int assetId, int userId, int portfolioId, string assetType);

    }
}