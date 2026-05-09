using Application_Service.Common;
using Application_Service.Mapper_s.AnnouncementMappers;
using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using Application_Service.Services.AnnouncementServices.Interfaces;
using Domain_Service.Entities.Shared;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.AnnouncementServices.Implementation
{
    public class AnnouncmentService : IAnnouncmentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IAudienceNotificationResolverService audienceService;
        public AnnouncmentService(IUnitOfWork uow, IAudienceNotificationResolverService audienceService)
        {
            _uow = uow;
            this.audienceService = audienceService;
        }

        public async Task<ApiResponse<string>> CreateAnnouncementAsync(AnnouncementRequest request, Guid postedBy)
        {
            try
            {
                // 🔹 1. Resolve audience users
                var userIds = await audienceService.ResolveUsersAsync(request);

                if (userIds == null || !userIds.Any())
                {
                    return ApiResponse<string>.Fail(
                        "No users found for selected audience",
                        ResponseType.NotFound);
                }

                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    // 🔹 2. Create Announcement
                    var announcement = request.MapToAnnouncement(postedBy);
                    await _uow.AnnouncmentRepo.CreateAsync(announcement);

                    // 🔹 3. Create Notifications for each user (FIXED)
                    var notifications = userIds.Select(userId =>
                    {
                        var notificationId = Guid.NewGuid(); // Create ID first
                        return new Notification
                        {
                            NotificationId = notificationId,
                            TargetUserId = userId,
                            SenderUserId = postedBy,
                            TargetRole = GetTargetRole(request.AnnouncementTargetAudience),
                            SessionId = request.SessionId,
                            DepartmentId = request.DepartmentId,
                            AnnouncementId = announcement.AnnouncmentId,
                            Title = request.Title,
                            Message = request.Message,
                            Type = NotificationType.Announcement,
                            IsRead = false,
                            CreatedAt = DateTime.UtcNow,
                            ActionUrl = $"/notification-view/{notificationId}" // Use the created ID
                        };
                    }).ToList();

                    await _uow.NotificationRepo.AddRangeAsync(notifications);
                });

                // 🔹 4. Optional Email Notification (outside transaction)
                if (request.SendMailNotification)
                {
                    // Uncomment and implement when email service is ready
                    // var users = await _uow.UserRepo
                    //     .Query()
                    //     .Where(u => userIds.Contains(u.UserId))
                    //     .ToListAsync();
                    //
                    // foreach (var user in users)
                    // {
                    //     try
                    //     {
                    //         await _emailService.SendEmailAsync(
                    //             user.Email,
                    //             request.Title,
                    //             request.Message
                    //         );
                    //     }
                    //     catch
                    //     {
                    //         // log failure but DO NOT break flow
                    //     }
                    // }
                }

                return ApiResponse<string>.Success(
                    "Announcement created successfully",
                    "Announcement published",
                    ResponseType.Created);
            }
            catch (Exception ex)
            {
                // Consider logging the exception here
                return ApiResponse<string>.Fail(
                    "Failed to create announcement",
                    ResponseType.InternalServerError);
            }
        }

        // 🔹 Map AnnouncementTargetAudience → RoleType?
        private RoleType? GetTargetRole(AnnouncementTargetAudience audience)
        {
            return audience switch
            {
                AnnouncementTargetAudience.FacultiesOnly => RoleType.Faculty,
                AnnouncementTargetAudience.StudentsOnly => RoleType.Student,
                AnnouncementTargetAudience.ClerksOnly => RoleType.Clerk,
                AnnouncementTargetAudience.Everyone => null, // broadcast to all roles
                _ => null
            };
        }
    }
}
