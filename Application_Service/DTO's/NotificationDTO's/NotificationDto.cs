using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.NotificationDTO_s
{

    public record NotificationDto(
        Guid NotificationId,
        string Title,
        string Message,
        string NotificationType,                 // e.g. "Announcement" Fee, Exam , FYP 
        string? AnnouncementType,    // e.g. "Urgent", "Event", "Information", "Deadline"
        bool IsRead,
        DateTime CreatedAt,
        string? ActionUrl,
        string SenderName
    );
}
