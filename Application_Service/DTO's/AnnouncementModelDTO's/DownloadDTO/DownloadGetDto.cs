using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.AnnouncementModelDTO_s.DownloadDTO
{
public record  DownloadGetDto(Guid FileId, string Title, string FilePath, Guid UploadedBy, DateTime DateUploaded);

}
