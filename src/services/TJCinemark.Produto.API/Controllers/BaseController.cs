using Microsoft.AspNetCore.Mvc;
using Microsoft.AspNetCore.Mvc.ModelBinding;
using TJ.ProductManagement.Domain.ErrorNotificator;
using TJ.ProductManagement.Domain.Interfaces.Services;

namespace TJ.ProductManagement.API.Controllers
{
    [ApiController]
    public class BaseController : ControllerBase
    {
        private readonly INotificator _notificator;

        protected BaseController(INotificator notificator)
        {
            _notificator = notificator ?? throw new ArgumentNullException(nameof(notificator));
        }

        protected bool ValidOperation() => !_notificator.HasNotification();

        protected IActionResult CustomResponse(object result = null)
        {
            if (ValidOperation())
                return Ok(new
                {
                    success = true,
                    data = result
                });

            return BadRequest(new
            {
                success = false,
                errors = _notificator.GetNotifications().Select(n => n.Message)
            }); ;
        }

        protected IActionResult CustomResponse(ModelStateDictionary modelState)
        {
            if (!modelState.IsValid) NotificateErrorInvalidModel(modelState);

            return CustomResponse();
        }

        protected void NotificateErrorInvalidModel(ModelStateDictionary modelState)
        {
            var errors = modelState.Values.SelectMany(e => e.Errors);
            foreach (var error in errors)
            {
                var errorMessage = error.Exception == null ? error.ErrorMessage : error.Exception.Message;
                NotificateError(errorMessage);
            }
        }

        protected void NotificateError(string mensagem) => _notificator.Handle(new Notification(mensagem));
        
        protected bool ValidateIdHexadecimal(string id)
        {
            bool isHex;

            foreach (var c in id)
            {
                isHex = ((c >= '0' && c <= '9') ||
                         (c >= 'a' && c <= 'f') ||
                         (c >= 'A' && c <= 'F'));

                if (!isHex) return false;
            }

            return true;
        }
    }
}
