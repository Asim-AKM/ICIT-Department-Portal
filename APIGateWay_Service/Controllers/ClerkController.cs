using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Services.AdminServices.Interfaces;
using Application_Service.Services.StudentServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// Handles operations related to clerical tasks, including uploading bulk student data for processing in the current
    /// session.
    /// </summary>
    /// <remarks>This controller is responsible for managing student-related operations and requires services
    /// for session and student management.</remarks>
    /// 

    
    [Route("api/[controller]")]
    [ApiController]
    public class ClerkController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IStudentService _studentService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionService"></param>
        /// <param name="studentService"></param>
        public ClerkController(ISessionService sessionService, IStudentService studentService)
        {
            // Constructor logic can be added here if needed
            _sessionService = sessionService;
                _studentService = studentService;

        }

        /// <summary>
        /// Uploads a bulk data file containing student information and processes the data for the current session.
        /// </summary>
        /// <remarks>This method handles the upload of student data in bulk and may return various HTTP
        /// status codes based on the result of the operation.</remarks>
        /// <param name="file">The file containing the bulk student data to be uploaded. This parameter must not be null and should be in a
        /// supported Excel format.</param>
        /// <param name="request">The session details required to process the upload. This parameter must not be null and should contain valid
        /// session information.</param>
        /// <param name="SessionId">Session Id Required</param>
        /// <returns>An IActionResult that indicates the outcome of the upload operation. The result may represent success,
        /// validation errors, conflicts, or server errors.</returns>
        [HttpPost("Upload-Student-BulkData")]
        [Consumes("multipart/form-data")]
        public async Task<IActionResult> UploadStudentBulkData(IFormFile file ,[FromForm] UploadBulkStudentDto request)
        {
            var response = await _studentService.UploadStudentsFromExcelAsync(request, file);

            return StatusCode((int)response.Status, response);
        }

    }
}
