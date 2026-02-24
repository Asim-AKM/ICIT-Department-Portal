using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.AnnouncementAndDownload
{
    public class Announcement
    {
        [Key]
        public Guid AnnouncementId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Content { get; set; } = string.Empty;
        public Guid PostedBy { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
