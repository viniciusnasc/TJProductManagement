using TJ.ProductManagement.Domain.ErrorNotificator;

namespace TJ.ProductManagement.Domain.Interfaces.Services
{
    public interface INotificator
    {
        bool HasNotification();
        List<Notification> GetNotifications();
        void Handle(Notification notificacao);
    }
}
