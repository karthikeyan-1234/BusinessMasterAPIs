using CommonLibrary;

namespace MaterialLibrary.Services
{
    public interface IMaterialChangeNotification
    {
        Task SendMaterialChangeNotification(string message, NotificationType notification);
        Task SendMaterialTypeChangeNotification(string message,NotificationType notification);
    }
}