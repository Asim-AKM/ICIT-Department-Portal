using Application_Service.Services.StudentServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{
    [Authorize (Roles ="Student")]
    [Route("api/[controller]")]
    [ApiController]
    public class StudentController : ControllerBase
    {
        private readonly ITranscriptService _transcriptService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="transcriptService"></param>
        public StudentController(ITranscriptService transcriptService)
        {
            _transcriptService = transcriptService;
        }


        [HttpGet("my-transcript")]
        public async Task<IActionResult> GetMyTranscript()
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;

            var response = await _transcriptService.GetStudentTranscriptAsync(Guid.Parse(userId!));
            return StatusCode((int)response.Status, response);
        }
    }
}
