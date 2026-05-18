using Application_Service.RequestAndResponseModel.GradeManagmentModels;
using Application_Service.Services.FacultyServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class FacultyController : ControllerBase
    {
        IFacultyService _facultyService;
        public FacultyController(IFacultyService facultyService)
        {
            _facultyService = facultyService;
        }

        [HttpGet("get-faculty-by-departmentId")]
        public async Task<IActionResult> GetFaculty([FromQuery] Guid departmentId)
        {
            var response=  await _facultyService.GetFacultiesByDepartmentAsync(departmentId);
            return StatusCode((int)response.Status, response);
        }
        // 🔹 1. My Subjects
        [HttpGet("my-subjects")]
        public async Task<IActionResult> GetMySubjects()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _facultyService.GetMySubjectsAsync(Guid.Parse( userId!));
            return StatusCode((int)response.Status, response);
        }
        // 🔹 2. Enrolled Students
        [HttpGet("get-enrolled-students")]
        public async Task<IActionResult> GetEnrolledStudents([FromQuery] Guid subjectId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _facultyService.GetEnrolledStudentsAsync(subjectId,Guid.Parse(userId!));
            return StatusCode((int)response.Status, response);

        }
        // 🔹 3. Assign Grade
        [HttpPost("assign-grade")]
        public async Task<IActionResult> AssignGrade([FromBody] CreateOrUpdateGradeRequest request)
        {
            var facultyid = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _facultyService.AssignGradeAsync(request,Guid.Parse(facultyid!));
            return StatusCode((int)response.Status, response);
        }

    }
}
