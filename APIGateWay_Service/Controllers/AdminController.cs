using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Services.AdminServices.Interfaces;
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

        /// <summary>
        /// Initializes a new instance of the <see cref="AdminController"/> class.
        /// </summary>
        /// <param name="sessionService">Service to manage session-related operations.</param>
        public AdminController(ISessionService sessionService)
        {
            _sessionService = sessionService;
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
    }
}