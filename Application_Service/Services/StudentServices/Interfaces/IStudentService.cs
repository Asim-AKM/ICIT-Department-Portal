using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using Domain_Service.Enum;
using Microsoft.AspNetCore.Http;

namespace Application_Service.Services.StudentServices.Interfaces
{
    public interface IStudentService
    {
        Task<ApiResponse<string>> UploadStudentsFromExcelAsync(UploadBulkStudentDto request, IFormFile file);
        Task<ApiResponse<List<GetStudentDto>>> GetStudentListBySessionIdAndDeprtIdAsync(GetStudentBySessionRequest getStudentBySession);
        Task<ApiResponse<string>> VerifyStudentAsync(StudentVerifyRequest studentVerifyRequest);
        Task<ApiResponse<BulkVerifyResultResponse>> VerifyStudentsBulkAsync(StudentBulkVerifyRequest bulkVerifyRequest);

    }
}
