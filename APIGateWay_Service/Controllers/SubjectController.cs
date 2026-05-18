using Application_Service.DTO_s.SubjectDTO_s;
using Application_Service.RequestAndResponseModel.SubjectManagmengModels;
using Application_Service.Services.SubjectServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SubjectController : ControllerBase
    {
        private readonly ISubjectService _subjectService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="subjectService"></param>
        public SubjectController(ISubjectService subjectService)
        {

            _subjectService = subjectService;
        }

        [HttpPost("create-subject")]
        public async Task<IActionResult> CreateSubject([FromBody] CreateSubjectRequest request)
        {
            var result = await _subjectService.CreateSubject(request);
            return StatusCode((int)result.Status, result);
        }

        // 🔹 Update Subject
        [HttpPut("update-subject")]
        public async Task<IActionResult> UpdateSubject(
            [FromBody] UpdateSubjectRequest request)
        {
            var result = await _subjectService.UpdateSubject(request);
            return StatusCode((int)result.Status, result);
        }

        // 🔹 Get All Subjects (Paginated)
        [HttpGet("get-all-subjects")]
        public async Task<IActionResult> GetAllSubjects( [FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var result = await _subjectService.GetAllSubject(pageNumber, pageSize);
            return StatusCode((int)result.Status, result);
        }
        // 🔹 Get Subjects by Department and Semester
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <param name="pageNumber"></param>
        /// <param name="pageSize"></param>
        /// <returns></returns>
        [HttpGet("get-subjects-by-department-and-semester")]
        public async Task<IActionResult> GetSubjectsByDepartmentAndSemester([FromQuery] Guid departmentId, [FromQuery] Guid semesterId, [FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {

            var result = await _subjectService.GetSubjectsByDepartmentAndSemester( departmentId,semesterId,pageNumber,pageSize);
            return StatusCode((int)result.Status, result);
        }

        // 🔹 Delete Subject (Soft Delete)
        [HttpDelete("delete-subject/{subjectId}")]
        public async Task<IActionResult> DeleteSubject(Guid subjectId)
        {
            var result = await _subjectService.DeleteSubject(subjectId);
            return StatusCode((int)result.Status, result);
        }

    }
}
