using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.StudentModels;
using Application_Service.Services.AdminServices.Interfaces;
using Application_Service.Services.StudentServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// Admin controller responsible for managing sessions and student verification.
    /// Provides endpoints for creating sessions, retrieving sessions,
    /// fetching students by session, and verifying students individually or in bulk.
    /// </summary>

    [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status200OK)]
    [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status500InternalServerError)]
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISessionService _sessionService;
        private readonly IStudentService _studentService;

        /// <summary>
        /// Constructor to inject required services.
        /// </summary>
        /// <param name="sessionService">Service responsible for session operations.</param>
        /// <param name="studentService">Service responsible for student operations.</param>
        public AdminController(ISessionService sessionService, IStudentService studentService)
        {
            _sessionService = sessionService;
            _studentService = studentService;
        }

        /// <summary>
        /// Creates a new session.
        /// </summary>
        /// <param name="request">Session creation request data.</param>
        /// <returns>Returns the created session response.</returns>
        [HttpPost("CreateSession")]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status409Conflict)]
        public async Task<IActionResult> CreateSession(SessionAddDto request)
        {
            var response = await _sessionService.CreateSessionAsync(request);
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieves all active sessions.
        /// </summary>
        /// <returns>List of active sessions.</returns>
        [HttpGet("Sessions")]
        public async Task<IActionResult> GetSessions()
        {
            var response = await _sessionService.GetActiveSessionsAsync();
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Retrieves students based on a specific session and status.
        /// </summary>
        /// <param name="getStudentBySession">Session ID and student status filter.</param>
        /// <returns>List of students for the specified session.</returns>
        [HttpGet("students-by-session")]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status404NotFound)]
        public async Task<IActionResult> GetStudentsBySessionAsync([FromQuery] GetStudentBySessionRequest getStudentBySession)
        {
            var response = await _studentService.GetStudentListBySessionIdAsync(getStudentBySession);
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Verifies a single student.
        /// Creates user account, credentials, and assigns student role.
        /// </summary>
        /// <param name="studentVerifyRequest">Student verification request.</param>
        /// <returns>Verification result.</returns>
        [HttpPut("Student-Verify")]
        public async Task<IActionResult> StudentVerify([FromBody] StudentVerifyRequest studentVerifyRequest)
        {
            var response = await _studentService.VerifyStudentAsync(studentVerifyRequest);
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// Verifies multiple students in bulk.
        /// Only unverified students will be processed.
        /// Already verified students will be skipped.
        /// </summary>
        /// <param name="bulkVerifyRequest">Bulk verification request containing student IDs and status.</param>
        /// <returns>Bulk verification result.</returns>
        [HttpPut("Student-Bulk-Verify")]
        public async Task<IActionResult> BulkStudentsVerify([FromBody] StudentBulkVerifyRequest bulkVerifyRequest)
        {
            var response = await _studentService.VerifyStudentsBulkAsync(bulkVerifyRequest);
            return StatusCode((int)response.Status, response);
        }
    }
}