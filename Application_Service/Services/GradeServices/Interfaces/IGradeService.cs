using Application_Service.Common;
using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Domain_Service.Entities.Academic;

namespace Application_Service.Services.GradeServices.Interfaces
{
    public  interface IGradeService
    {
        Task<ApiResponse<string>> CreateOrUpdateGradeAsync(CreateOrUpdateGradeRequest request);
        Task<ApiResponse<Grade>> GetGradeByEnrollmentAsync(Guid enrollmentId);
        Task<ApiResponse<List<Grade>>> GetStudentTranscriptAsync(Guid studentId);
    }
}
