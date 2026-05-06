using Application_Service.Common;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Services.UserManagmentServices.Implementation;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;


namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// Controller for handling user account operations such as creating new accounts.
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AccountController : ControllerBase
    {
        private readonly IAccounService _accounService;

        /// <summary>
        /// Initializes a new instance of the <see cref="AccountController"/> class.
        /// </summary>
        /// <param name="accounService">Injected account service for handling business logic.</param>
        public AccountController(IAccounService accounService)
        {
            _accounService = accounService;
        }

        /// <summary>
        /// Creates a new user account with the specified role.
        /// </summary>
        /// <param name="request">The user details including full name, username, email, password, and role.</param>
        /// <returns>
        /// Returns an <see cref="IActionResult"/> containing the status code and response object indicating success or failure.
        /// </returns>
        /// <remarks>
        /// Sample request:
        ///
        /// POST /api/Account/Account
        /// {
        ///     "fullName": "Asim Khan",
        ///     "userName": "Akm",
        ///     "email": "akm@example.com",
        ///     "password": "YourPassword123",
        ///     "role": "Admin"
        /// }
        /// </remarks>
        [Authorize(Roles ="Admin")]
        [HttpPost("Account")]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status400BadRequest)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status409Conflict)]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> CreateAccount([FromBody] CreateUserDto request)
        {
            var response = await _accounService.CreateAccount(request, request.Role);
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>
        [Authorize]
        [HttpGet("Profile")]
        [ProducesResponseType(typeof(ApiResponse<GetUserProfileDto>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<GetUserProfileDto>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> GetProfileDetails()
        {
          
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _accounService.GetProfileDetails(Guid.Parse(userId!));
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>

        [Authorize]
        [HttpPut("UploadProfileImage")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> UploadProfileImage(UserProfileImageUploadDto request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            request.UserId = Guid.Parse(userId!);
            var response = await _accounService.UploadProfileImage(request);
            return StatusCode((int)response.Status, response);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="userId"></param>
        /// <returns></returns>
        [Authorize]
        [HttpDelete("remove-profile-image/{userId}")]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(ApiResponse<string>), StatusCodes.Status500InternalServerError)]
        public async Task<IActionResult> RemoveProfileImage(Guid userId)
        {
            var response = await _accounService.RemoveProfileImage(userId);
            return StatusCode((int)response.Status, response);
        }


    }
}