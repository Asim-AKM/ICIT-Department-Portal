using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.AnnouncementServices.Interfaces
{
    public interface IAudienceNotificationResolverService
    {
        Task<List<Guid>> ResolveUsersAsync(AnnouncementRequest request);
    }
}
