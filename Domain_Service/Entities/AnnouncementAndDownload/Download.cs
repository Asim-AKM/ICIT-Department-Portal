using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.AnnouncementAndDownload
{
    public class Download
    {
       public Guid FileId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string FilePath { get; set; } = string.Empty;
        public Guid UploadedBy { get; set; }
        public DateTime DateUploaded { get; set; }
    }
}
