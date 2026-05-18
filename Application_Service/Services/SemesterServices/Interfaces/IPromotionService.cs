using Application_Service.Common;
using Application_Service.DTO_s.SemesterDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.SemesterServices.Interfaces
{
    public interface IPromotionService
    {
        Task<ApiResponse<PromotionResultDto>> SingleStudentSemesterPromotionAsync(PromotionRequest request);
        Task<ApiResponse<List<PromotionResultDto>>> FullBatchSemesterPromotionAsync(BatchPromotionRequest request);
    }
}
