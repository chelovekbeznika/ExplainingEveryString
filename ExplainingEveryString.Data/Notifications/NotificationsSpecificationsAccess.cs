using System;
using System.Collections.Generic;
using System.Text;

namespace ExplainingEveryString.Data.Notifications
{
    public static class NotificationsSpecificationsAccess
    {
        public static NotificationSpecification[] Load()
        {
            return JsonDataAccessor.Instance.Load<NotificationSpecification[]>(FileNames.Notifications);
        }
    }
}
