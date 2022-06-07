using Microsoft.Extensions.Hosting;
using Microsoft.Extensions.Logging;
using Microsoft.Extensions.DependencyInjection;
using System;
using System.Threading;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using ApplicationCore.Entity;
using ApplicationCore.NotificationAggregate.DTOs;
using System.Linq;
using Mapster;

namespace ApplicationCore.NotificationAggregate
{
    public class NotificationService : INotificationService
    {
        private readonly IBaseRepository<Notification> _notificationRepository;



        public NotificationService(IBaseRepository<Notification> notificationRepository)
        {
            _notificationRepository = notificationRepository;
        }
        public Notification RegisterPriceNotification(int userId, int portfolioId, NotificationDto dto, bool isHigh)

        {
            var notification = dto.Adapt<Notification>();
            notification.UserId = userId;
            notification.PortfolioId = portfolioId;
            if (isHigh)
                notification.IsHighOn = true;
            else
                notification.IsLowOn = true;
            _notificationRepository.Insert(notification);
            return notification;
        }

        public Notification EditNotification(int id, EditNotificationDto dto)
        {
            var found = _notificationRepository.GetFirst((item) => item.Id == id);
            if (found == null)
            {
                return null;
            }
            found.HighThreadHoldAmount = dto.HighThreadHoldAmount;
            found.LowThreadHoldAmount = dto.LowThreadHoldAmount;
            found.IsHighOn = dto.IsHighOn;
            found.IsLowOn = dto.IsLowOn;
            _notificationRepository.Update(found);
            return found;

        }
        public List<Notification> GetActiveHighNotifications(string assetType)
        {
            return _notificationRepository.List((item) => item.AssetType == assetType && item.IsHighOn == true).ToList();
        }

        public List<Notification> GetActiveLowNotifications(string assetType)
        {
            return _notificationRepository.List((item) => item.AssetType == assetType && item.IsLowOn == true).ToList();
        }


        public Notification GetNotificationByAssetIdAndAssetType(int assetId, int userId, int portfolioId, string assetType)
        {
            return _notificationRepository.GetFirst((item) => item.AssetId == assetId && item.AssetType == assetType && item.UserId == userId && item.PortfolioId == portfolioId);
        }

        public void TurnOffHighNotificationById(int id)
        {
            var found = _notificationRepository.GetFirst((item) => item.Id == id);
            if (found != null)
            {
                found.IsHighOn = false;
                _notificationRepository.Update(found);
            }
        }

        public void TurnOffLowNotificationById(int id)
        {
            var found = _notificationRepository.GetFirst((item) => item.Id == id);
            if (found != null)
            {
                found.IsLowOn = false;
                _notificationRepository.Update(found);
            }
        }
    }
}
