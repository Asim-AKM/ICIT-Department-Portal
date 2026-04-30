using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

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
            return StatusCode((int)response.Status, response);
        }

    }
}
