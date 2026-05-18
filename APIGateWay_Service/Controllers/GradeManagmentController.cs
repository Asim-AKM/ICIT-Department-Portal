using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Application_Service.Services.GradeServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class GradeManagmentController : ControllerBase
    {

        private readonly IGradeService _gradeService;
        public GradeManagmentController(IGradeService gradeService)
        {
            _gradeService = gradeService;
        }

        // 🔹 Create or Update Grade
        [HttpPost("save-grade")]
        public async Task<IActionResult> SaveGrade([FromBody] CreateOrUpdateGradeRequest request)
        {
            var response = await _gradeService.CreateOrUpdateGradeAsync(request);
            return StatusCode((int)response.Status, response);
        }

        // 🔹 Get Grade by Enrollment
        [HttpGet("get-by-enrollment/{enrollmentId}")]
        public async Task<IActionResult> GetGradeByEnrollment(Guid enrollmentId)
        {
            var response = await _gradeService.GetGradeByEnrollmentAsync(enrollmentId);
            return StatusCode((int)response.Status, response);
        }

        // 🔹 Get Student Transcript (All Grades)
        [HttpGet("get-transcript/{studentId}")]
        public async Task<IActionResult> GetTranscript(Guid studentId)
        {
            var response = await _gradeService.GetStudentTranscriptAsync(studentId);
            return StatusCode((int)response.Status, response);
        }
    }
}




