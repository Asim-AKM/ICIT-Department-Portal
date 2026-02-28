using Microsoft.AspNetCore.Http;

namespace Application_Service.Services.StudentServices.Interfaces
{
    public interface IStudentService
    {
        Task<string> UploadStudentsFromExcelAsync(IFormFile file);
    }
}
