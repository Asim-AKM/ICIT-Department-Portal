using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using Domain_Service.Enum;
using Microsoft.AspNetCore.Http;

namespace Application_Service.Services.StudentServices.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponse<string>> UploadStudentsFromExcelAsync(IFormFile file, Guid SessionId);
        Task<ApiResponse<List<GetStudentDto>>> GetStudentListBySessionIdAsync(Guid SessionId);
        Task<ApiResponse<string>> VerifyStudentAsync(Guid studentId,StudentStatus studentStatus);
        Task<ApiResponse<BulkVerifyResultResponse>> VerifyStudentsBulkAsync(BulkVerifyRequest bulkVerifyRequest);

    }
}
