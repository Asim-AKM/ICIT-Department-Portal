using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Services.AdminServices.Interfaces;
using Application_Service.Services.StudentServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// Controller for administrative operations 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AdminController : ControllerBase
    {
        private readonly ISessionService _sessionService;

        private readonly IStudentService _studentService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="sessionService">Service to manage session-related operations.</param>
        public AdminController(ISessionService sessionService,IStudentService studentService)
        {
            _sessionService = sessionService;
            _studentService = studentService;
        }

        /// <summary>
        /// Creates a new academic session.
        /// </summary>
        /// <param name="request">The session data transfer object containing session details.</param>
        /// <returns>
        /// Returns a response object with status code:
        /// <list type="bullet">
        /// <item><description>200 OK - if the session is created successfully.</description></item>
        /// <item><description>400 Bad Request - if the input data is invalid.</description></item>
        /// <item><description>409 Conflict - if a session with the same details already exists.</description></item>
        /// <item><description>500 Internal Server Error - if an unexpected error occurs.</description></item>
        /// </list>
        /// </returns>
        [HttpPost("CreateSession")]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateSession(SessionAddDto request)
        {
            var response = await _sessionService.CreateSessionAsync(request);
            return StatusCode((int)response.Status, response);
        }

        [HttpGet("Sessions")]
        [ProducesResponseType(typeof(SessionGetDTO), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(SessionGetDTO), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(SessionGetDTO), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(SessionGetDTO), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetSessions()
        {
            var response = await _sessionService.GetAllSessionsAsync();
            return StatusCode((int)response.Status, response);
        }




        /// <summary>
        /// Retrieves the list of students associated with the specified session.
        /// </summary>
        /// <remarks>Returns a 400 Bad Request if the sessionId is invalid, a 404 Not Found if no students
        /// are associated with the session, or a 500 Internal Server Error for unexpected failures.</remarks>
        /// <param name="sessionId">The unique identifier of the session for which to retrieve students. This parameter must not be null.</param>
        /// <returns>An IActionResult containing an ApiResponse with a list of GetStudentDto objects. The response indicates the
        /// result of the operation.</returns>
        [HttpGet("students-by-session")]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status404NotFound)]
        [ProducesResponseType(typeof(ApiResponse<List<GetStudentDto>>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetStudentsBySessionAsync([FromQuery] Guid sessionId)
        {
            var response = await _studentService.GetStudentListBySessionIdAsync(sessionId);
            return StatusCode((int)response.Status, response);
        }
    }
}