using Application_Service.RequestAndResponseModel.StudentModels;
using Application_Service.Services.SemesterServices.Interfaces;
using Microsoft.AspNetCore.Authorization;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Mvc;
using System.Security.Claims;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// Provides API endpoints for promoting students to the next semester, either individually or in batches.
    /// </summary>
    /// <remarks>This controller requires authentication and is intended for use by authorized users managing
    /// student promotions. It exposes endpoints for evaluating and processing semester promotions for single students
    /// as well as entire batches. All actions are accessible via HTTP POST requests and expect the relevant promotion
    /// request data in the request body.</remarks>
    [Authorize]
    [Route("api/[controller]")]
    [ApiController]
   
    public class SemesterPromotionController : ControllerBase
    {
        private readonly IPromotionService _promotionService;
        public SemesterPromotionController(IPromotionService promotionService)
        {
            _promotionService = promotionService;
        }

        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("single-student-semester-promotion")]
        public async Task<IActionResult> EvaluatePromotion([FromBody] PromotionRequest request)
        {
            var userId = User.FindFirst(ClaimTypes.NameIdentifier)?.Value;
            var response = await _promotionService.SingleStudentSemesterPromotionAsync(request);
            return StatusCode((int)response.Status, response);
        }
        /// <summary>
        /// 
        /// </summary>
        /// <param name="request"></param>
        /// <returns></returns>
        [HttpPost("full-batch-semester-Promotion")]
        public async Task<IActionResult> BatchPromotion([FromBody] BatchPromotionRequest request )
        {
            var response = await _promotionService.FullBatchSemesterPromotionAsync(request);
            return StatusCode((int)response.Status, response);
        }
    }
}
