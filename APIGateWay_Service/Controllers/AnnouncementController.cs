using Application_Service.RequestAndResponseModel.AnnouncementRequestModel;
using Application_Service.Services.AnnouncementServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class AnnouncementController : ControllerBase
    {

        private readonly IAnnouncmentService _announcementService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="announcmentService"></param>
        public AnnouncementController(IAnnouncmentService announcmentService)
        {
            _announcementService = announcmentService;  
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [Authorize]
        [HttpPost("create")]
        public async Task<IActionResult> CreateAnnouncement([FromBody] AnnouncementRequest request)
        {
            var postedBy = Guid.Parse(User.FindFirst(ClaimTypes.NameIdentifier)!.Value);
            var response = await _announcementService.CreateAnnouncementAsync(request, postedBy);
            return StatusCode((int)response.Status, response);

        }
    }
}
