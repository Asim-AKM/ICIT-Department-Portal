using Application_Service.DTO_s.UserManagmentDTO_s;
using Application_Service.Services.DeptServices.Interfaces;
using Microsoft.AspNetCore.Mvc;

namespace APIGateway_Service.Controllers
{
    /// <summary>
    /// 
    /// </summary>
    public class DepartmentController : Controller
    {
        IDepartmentService _departmentService;
        /// <summary>
        /// /// Initializes a new instance of the <see cref="DepartmentController"/> class.
        /// </summary>
        /// <param name="department"></param>
        public DepartmentController(IDepartmentService department)
        {
            _departmentService = department;
        }
        /// <summary>
        /// Retrieves a list of departments.
        /// </summary>
        /// <returns>A view displaying the list of departments.</returns>
        [HttpGet("Departments")]
        [ProducesResponseType(typeof(CreateUserDto), StatusCodes.Status400BadRequest)]

        public async Task<IActionResult> Departments()
        {
             var response = await _departmentService.GetDepartmentsAsync();
            return StatusCode((int)response.Status, response);
        }
    }
}
