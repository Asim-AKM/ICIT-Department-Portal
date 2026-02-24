using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.AnnouncementAndDownload
{
    public class Download
    {
        [Key]
        public Guid FileId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public Guid UploadedBy { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
