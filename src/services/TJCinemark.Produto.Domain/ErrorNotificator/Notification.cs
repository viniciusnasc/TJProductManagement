namespace TJ.ProductManagement.Domain.ErrorNotificator
{
    public class Notification
    {
        public Notification(string message)
        {
            Message = message;
        }

        public string Message { get; }
    }
}
