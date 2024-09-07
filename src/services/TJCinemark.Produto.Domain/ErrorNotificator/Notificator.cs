using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using TJ.ProductManagement.Domain.Interfaces.Services;

namespace TJ.ProductManagement.Domain.ErrorNotificator
{
    public class Notificator : INotificator
    {
        private List<Notification> _notifications;

        public Notificator() => _notifications = new List<Notification>();
        
        public void Handle(Notification notificacao) => _notifications.Add(notificacao);
        
        public List<Notification> GetNotifications() => _notifications;

        public bool HasNotification() => _notifications.Any();
    }
}
