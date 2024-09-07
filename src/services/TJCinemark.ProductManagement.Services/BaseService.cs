using System.ComponentModel.DataAnnotations;
using TJ.ProductManagement.Domain.ErrorNotificator;
using TJ.ProductManagement.Domain.Interfaces.Services;

namespace TJ.ProductManagement.Services
{
    public class BaseService
    {
        private readonly INotificator _notificador;

        protected BaseService(INotificator notificador)
        => _notificador = notificador;
        
        protected void Notificate(string message) => _notificador.Handle(new Notification(message));
    }
}
