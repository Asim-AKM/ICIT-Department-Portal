using Application_Service.Services.SemesterServices.Interfaces;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    [Route("api/[controller]")]
    [ApiController]
    public class SemesterController : ControllerBase
    {
        ISemesterService _semesterService;
        /// <summary>
        /// 
        /// </summary>
        /// <param name="semesterService"></param>
        public SemesterController(ISemesterService semesterService)
        {
            _semesterService = semesterService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="sessionId"></param>
        /// <returns></returns>

        [HttpGet("get-Semester-by-sessionId")]
        public async Task<IActionResult> GetSemester([FromQuery] Guid sessionId)
        {
            var response = await _semesterService.GetSemestersAsync(sessionId);
            return StatusCode((int)response.Status, response);
        }
    }
}
