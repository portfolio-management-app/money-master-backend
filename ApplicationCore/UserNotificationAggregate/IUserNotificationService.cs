using ApplicationCore.Entity;
using System.Collections.Generic;


namespace ApplicationCore.UserNotificationAggregate
{
    public interface IUserNotificationService
    {
        public UserNotification InsertNewNotification(Notification notification, string notificationType);

        public List<UserNotification> GetListNotificationByUserId(int userId);

        public UserNotification ReadUserNotification(int id);
    }
}