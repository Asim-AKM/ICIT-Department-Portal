using Application_Service.Common;
using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.AnnouncementServices.Interfaces
{
    public interface IAnnouncmentService
    {
        Task<ApiResponse<string>> CreateAnnouncementAsync(AnnouncementRequest request, Guid postedBy);
    }
}
