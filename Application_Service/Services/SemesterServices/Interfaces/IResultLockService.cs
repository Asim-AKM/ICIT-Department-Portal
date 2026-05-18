using Application_Service.Common;
using Application_Service.RequestAndResponseModel.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SemesterServices.Interfaces
{
    public interface IResultLockService
    {
        Task<ApiResponse<string>> LockResultsAsync( LockResultRequest request,Guid lockedBy);

        Task<ApiResponse<string>> UnlockResultsAsync( Guid semesterId, Guid departmentId);

        Task<bool> IsResultLockedAsync( Guid semesterId,Guid departmentId);
    }
}
