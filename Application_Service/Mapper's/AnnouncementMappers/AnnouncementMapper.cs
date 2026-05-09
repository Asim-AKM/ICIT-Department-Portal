using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using Domain_Service.Entities.Shared;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Mapper_s.AnnouncementMappers
{
    public static class AnnouncementMapper
    {
        public static Announcement MapToAnnouncement(this AnnouncementRequest request, Guid postedBy)
        {
            return new Announcement
            {
                AnnouncmentId = Guid.NewGuid(),
                Title = request.Title,
                Message = request.Message,
                AnnouncementType = request.AnnouncementType,
                AnnouncementTargetAudience = request.AnnouncementTargetAudience,
                DepartmentId = request.DepartmentId,
                SessionId = request.SessionId,
                SendMailNotification = request.SendMailNotification,
                DatePosted = DateTime.UtcNow,
                PostedBy = postedBy,
                IsActive = true
            };
        }
    }
}

