using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AuthenticationController : ControllerBase
    {
        private readonly IAuthenticationServce _authService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="authenticationServce"></param>
        public AuthenticationController(IAuthenticationServce authenticationServce)
        {
            _authService = authenticationServce;
        }

        [HttpPost("Login")]
        [ProducesResponseType(typeof(UserLoginRequest), StatusCodes.Status200OK)]
        [ProducesResponseType(typeof(UserLoginRequest), StatusCodes.Status401Unauthorized)]
        public async Task<IActionResult> Login(UserLoginRequest request)
        {
            var response = await _authService.UserLogin(request);

            if(!response.IsSuccess)
            {

                return StatusCode((int)response.Status, response);
            }

            var token = response.Data;

            Response.Cookies.Append("jwt", token, new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None, // 🔥 ADD THIS
                Expires = DateTime.UtcNow.AddDays(1),
            });
            response.Data = null;
            return StatusCode((int)response.Status, response);

        }
        [HttpPost("Logout")]
        public IActionResult Logout()
        {
           
            var cookieOptions = new CookieOptions
            {
                HttpOnly = true,
                Secure = true,
                SameSite = SameSiteMode.None,
                Path = "/",
                Expires = DateTime.UtcNow.AddDays(-1)
            };

            // Delete existing cookie
            Response.Cookies.Delete("jwt", cookieOptions);

            // Append empty cookie
            Response.Cookies.Append("jwt", "", cookieOptions);

            return Ok(new { message = "Logged out successfully" });
        }

        [Authorize]
        [HttpGet("Me")]
        public IActionResult Me()
        {
            var user = new
            {
                userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value,
                fullName = User.FindFirst(ClaimTypes.Name)?.Value,
                email = User.FindFirst(ClaimTypes.Email)?.Value,
                role = User.FindFirst(ClaimTypes.Role)?.Value
            };

            return Ok(user);
        }

    }
}
