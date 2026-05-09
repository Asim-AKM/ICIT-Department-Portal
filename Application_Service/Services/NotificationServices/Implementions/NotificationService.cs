using Application_Service.Common;
using Application_Service.DTO_s.NotificationDTO_s;
using Application_Service.Services.NotificationServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.NotificationServices.Implementions
{
    public class NotificationService : INotificationService
    {

        private readonly IUnitOfWork _uow;

        public NotificationService(IUnitOfWork uow)
        {
            _uow = uow;
        }

        // ===============================================
        // GetMyNotificationsAsync
        // ===============================================
        public async Task<ApiResponse<List<NotificationDto>>> GetMyNotificationsAsync(Guid userId)
        {
            try
            {
                // 🔹 Validate userId
                if (userId == Guid.Empty)
                {
                    return ApiResponse<List<NotificationDto>>.Fail(
                        "Invalid user identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Verify user exists
                var userExists = await _uow.UserRepo.Query()
                    .AnyAsync(u => u.UserId == userId);

                if (!userExists)
                {
                    return ApiResponse<List<NotificationDto>>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 Get unread notifications only
                var notifications = await _uow.NotificationRepo.Query()
                    .Where(n =>
                        n.TargetUserId == userId &&
                        !n.IsRead
                    )
                    .OrderByDescending(n => n.CreatedAt)
                    .ToListAsync();

                // 🔹 Handle empty result
                if (!notifications.Any())
                {
                    return ApiResponse<List<NotificationDto>>.Success(
                        new List<NotificationDto>(),
                        "No unread notifications found",
                        ResponseType.Ok);
                }

                // 🔹 Get sender names
                var senderIds = notifications
                    .Where(n => n.SenderUserId.HasValue)
                    .Select(n => n.SenderUserId!.Value)
                    .Distinct()
                    .ToList();

                var senders = await _uow.UserRepo.Query()
                    .Where(u => senderIds.Contains(u.UserId))
                    .ToDictionaryAsync(u => u.UserId, u => u.FullName);

                // 🔹 Get linked announcements to resolve AnnouncementType
                var announcementIds = notifications
                    .Where(n => n.AnnouncementId.HasValue)
                    .Select(n => n.AnnouncementId!.Value)
                    .Distinct()
                    .ToList();

                var announcements = await _uow.AnnouncmentRepo.Query()
                    .Where(a => announcementIds.Contains(a.AnnouncmentId))
                    .ToDictionaryAsync(a => a.AnnouncmentId, a => a.AnnouncementType);

                // 🔹 Map to DTO
                var result = notifications.Select(n =>
                {
                    // Sender name
                    var senderName =
                        n.SenderUserId.HasValue &&
                        senders.ContainsKey(n.SenderUserId.Value)
                            ? senders[n.SenderUserId.Value]
                            : "System";

                    // AnnouncementType (only if linked announcement exists)
                    string? announcementType = null;

                    if (n.AnnouncementId.HasValue &&
                        announcements.ContainsKey(n.AnnouncementId.Value))
                    {
                        announcementType = announcements[n.AnnouncementId.Value].ToString();
                    }

                    return new NotificationDto(
                        n.NotificationId,
                        n.Title,
                        n.Message,
                        n.Type.ToString(),    // "Announcement" Notification enum Type convert to string 
                        announcementType,       // "Urgent", "Event", etc.
                        n.IsRead,
                        n.CreatedAt,
                        n.ActionUrl,
                        senderName
                    );
                }).ToList();

                return ApiResponse<List<NotificationDto>>.Success(
                    result,
                    "Notifications retrieved",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<NotificationDto>>.Fail(
                    "Failed to retrieve notifications",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<string>> MarkNotificationAsReadAsync(Guid notificationId, Guid userId)
        {
            try
            {
                // 🔹 Validate NotificationId
                if (notificationId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid notification identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate UserId
                if (userId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid user identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Get notification belonging to this user
                var notification = await _uow.NotificationRepo
                    .Query()
                    .FirstOrDefaultAsync(n =>
                        n.NotificationId == notificationId &&
                        n.TargetUserId == userId);

                // 🔹 Notification not found
                if (notification == null)
                {
                    return ApiResponse<string>.Fail(
                        "Notification not found",
                        ResponseType.NotFound);
                }

                // 🔹 Already marked as read
                if (notification.IsRead)
                {
                    return ApiResponse<string>.Success(
                        "Notification is already marked as read",
                        "Notification already read",
                        ResponseType.Ok);
                }

                // 🔹 Update status
                notification.IsRead = true;

                await _uow.NotificationRepo.Update(notification);
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success(
                    "Notification marked as read successfully",
                    "Notification updated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to mark notification as read",
                    ResponseType.InternalServerError);
            }
        }
        public async Task<ApiResponse<List<NotificationDto>>> GetRecentUserNotificationsAsync(Guid userId, int count = 5)
        {
            try
            {
                // 🔹 Validate
                if (userId == Guid.Empty)
                {
                    return ApiResponse<List<NotificationDto>>.Fail(
                        "Invalid user identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Check if user exists
                var userExists = await _uow.UserRepo.Query()
                    .AnyAsync(u => u.UserId == userId);

                if (!userExists)
                {
                    return ApiResponse<List<NotificationDto>>.Fail(
                        "User not found",
                        ResponseType.NotFound);
                }

                // 🔹 Get sender names for mapping
                var userDict = await _uow.UserRepo.Query()
                    .Select(u => new { u.UserId, u.FullName })
                    .ToDictionaryAsync(x => x.UserId, x => x.FullName);

                // 🔹 Get announcement types for mapping
                var announcementDict = await _uow.AnnouncmentRepo.Query()
                    .Select(a => new { a.AnnouncmentId, a.AnnouncementType })
                    .ToDictionaryAsync(x => x.AnnouncmentId, x => x.AnnouncementType.ToString());

                // 🔥 REFACTORED: ONLY direct user notifications
                var notifications = await _uow.NotificationRepo.Query()
                    .Where(n => n.TargetUserId == userId)  // Only this specific user
                    .OrderByDescending(n => n.CreatedAt)    // Latest first
                    .Take(count)                            // Take only 'count' (default 5)
                    .ToListAsync();

                // 🔹 Map to DTO
                var result = notifications.Select(n =>
                {
                    var announcementType = n.AnnouncementId.HasValue &&
                        announcementDict.ContainsKey(n.AnnouncementId.Value)
                            ? announcementDict[n.AnnouncementId.Value]
                            : null;

                    var senderName = n.SenderUserId.HasValue &&
                        userDict.ContainsKey(n.SenderUserId.Value)
                            ? userDict[n.SenderUserId.Value]
                            : "System";

                    return new NotificationDto(
                        n.NotificationId,
                        n.Title,
                        n.Message,
                        n.Type.ToString(),
                        announcementType,
                        n.IsRead,
                        n.CreatedAt,
                        n.ActionUrl,
                        senderName
                    );
                }).ToList();

                return ApiResponse<List<NotificationDto>>.Success(
                    result,
                    "Recent notifications retrieved successfully",
                    ResponseType.Ok);
            }
            catch (Exception ex)
            {
                // Log exception here if you have logging
                return ApiResponse<List<NotificationDto>>.Fail(
                    "Failed to retrieve notifications",
                    ResponseType.InternalServerError);
            }
        }
    }
}


