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
        public string Content { get; set; } = string.Empty;
        public Guid PostedBy { get; set; }
        public DateTime DatePosted { get; set; }
    }
}
