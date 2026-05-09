using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Shared
{
    public class Notification
    {
        [Key]
        public Guid NotificationId { get; set; }

        // OPTIONAL: link to announcement (VERY IMPORTANT for traceability)
        public Guid? AnnouncementId { get; set; }

        // Sender (Faculty / Clerk / Admin / System)
        public Guid? SenderUserId { get; set; }

        public string Title { get; set; } = string.Empty;

        public string Message { get; set; } = string.Empty;

        public NotificationType Type { get; set; }

        public DateTime CreatedAt { get; set; } = DateTime.UtcNow;

        public bool IsRead { get; set; } = false;

        public string? ActionUrl { get; set; }

        // =========================
        // 🔥 TARGETING SYSTEM
        // =========================
        public Guid? TargetUserId { get; set; }        // single user (optional)

        public Guid? DepartmentId { get; set; }        // whole department

        public Guid? SessionId { get; set; }           // session-wide

        public RoleType? TargetRole { get; set; }      // Students / Faculty / Clerk

        public bool IsBroadcast { get; set; } = false; // global announcement
    }
}
