using Application_Service.Services.NotificationServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{

    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
    public class NotificationController : ControllerBase
    {
        private readonly INotificationService _notificationService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationService"></param>
        public NotificationController(INotificationService notificationService)
        {
            _notificationService = notificationService;
        }

        // GET: api/Notification/my-notification
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("my-notification")]
        public async Task<IActionResult> GetMyNotifications()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var result = await _notificationService.GetMyNotificationsAsync(Guid.Parse(userId!));
            return StatusCode((int)result.Status, result);
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="notificationId"></param>
        /// <returns></returns>
        [HttpPut("mark-as-read-notification/{notificationId}")]
        [Authorize]
        public async Task<IActionResult> MarkAsRead(Guid notificationId)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _notificationService.MarkNotificationAsReadAsync(notificationId, Guid.Parse(userId!));
            return StatusCode((int)response.Status, response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <returns></returns>

        [HttpGet("recent")]
        [Authorize]
        public async Task<IActionResult> GetLatest()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _notificationService.GetRecentUserNotificationsAsync(Guid.Parse(userId!));
            return StatusCode((int)response.Status,response);
        }
    }
}
