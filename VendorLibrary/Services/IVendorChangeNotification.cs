using CommonLibrary;

namespace VendorLibrary.Services
{
    public interface IVendorChangeNotification
    {
        Task SendVendorChangeNotification(string message, NotificationType notificationType);
    }
}