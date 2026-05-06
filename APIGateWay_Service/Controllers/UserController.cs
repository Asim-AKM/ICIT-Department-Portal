using Application_Service.RequestAndResponseModel.AuthenticationModels;
using Application_Service.Services.UserManagmentServices.Interfaces;
using Azure;
using Domain_Service.Enum;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class UserController : ControllerBase
    {
        private readonly IUserService _userService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="userService"></param>
        public UserController(IUserService userService)
        {
            _userService = userService;
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [Authorize]
        [HttpGet("get-all-users")]
        public async Task<IActionResult> GetAllUsers([FromQuery] int pageNumber = 1,[FromQuery] int pageSize = 10)
        {
            var response = await _userService.GetAllUsers(pageNumber, pageSize);
            return StatusCode((int)response.Status, response);
        }


        /// <summary>
        /// 
        /// </summary>
        /// <param name="role"></param>
        /// <param name="status"></param>
        /// <returns></returns>
        [Authorize]
        [HttpGet("filter-users")]
        public async Task<IActionResult> GetUsersByFilter([FromQuery] GetUserByRoleAndStatusRequest request)
        {
            var response = await _userService.GetUsersByFilter(request);
            return StatusCode((int)response.Status, response);
        }
    }
}

