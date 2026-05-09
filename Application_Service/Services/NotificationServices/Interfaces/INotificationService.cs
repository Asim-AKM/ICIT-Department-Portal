using Application_Service.Common;
using Application_Service.DTO_s.NotificationDTO_s;

namespace Application_Service.Services.NotificationServices.Interfaces
{
    public interface INotificationService
    {
        Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(Guid userId);
        Task<ApiResponse<string>> MarkNotificationAsReadAsync(Guid notificationId, Guid userId);
        Task<ApiResponse<List<NotificationDto>>> GetRecentUserNotificationsAsync(Guid userId, int count = 5);
    }
}
