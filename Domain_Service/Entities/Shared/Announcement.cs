using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Shared
{
    public class Announcement
    {
        [Key]
        public Guid AnnouncmentId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Message { get; set; } = string.Empty;
        public AnnouncementType AnnouncementType { get; set; }
        public AnnouncementTargetAudience AnnouncementTargetAudience { get; set; }
        public bool SendMailNotification { get; set; }
        public Guid? DepartmentId { get; set; } 
        public Guid? SessionId { get; set; }
        public DateTime DatePosted { get; set; }
        public Guid PostedBy { get; set; }
        public bool IsActive { get; set; } = true;   // Soft Status
    }
}
