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

namespace ApplicationCore.UserNotificationAggregate
{
    public class UserNotificationService : IUserNotificationService
    {
        private readonly IBaseRepository<UserNotification> _userNotificationRepository;


        public UserNotificationService(IBaseRepository<UserNotification> userNotificationRepository)
        {
            _userNotificationRepository = userNotificationRepository;
        }


        public UserNotification InsertNewNotification(Notification notification, string notificationType)
        {
            var userNotification = new UserNotification()
            {
                UserId = notification.UserId,
                AssetId = notification.AssetId,
                PortfolioId = notification.PortfolioId,
                AssetType = notification.AssetType,
                AssetName = notification.AssetName,
                HighThreadHoldAmount = notification.HighThreadHoldAmount,
                LowThreadHoldAmount = notification.LowThreadHoldAmount,
                NotificationType = notificationType,
                Currency = notification.Currency,
            };
            _userNotificationRepository.Insert(userNotification);
            return userNotification;
        }

        public List<UserNotification> GetListNotificationByUserId(int userId)
        {
            return _userNotificationRepository.List((item) => item.UserId == userId).ToList();
        }

        public UserNotification ReadUserNotification(int id)
        {
            var found = _userNotificationRepository.GetFirst(item => item.Id == id);
            if (found != null)
            {
                found.IsRead = true;
                _userNotificationRepository.Update(found);
                return found;
            }
            return null;
        }

    }
}