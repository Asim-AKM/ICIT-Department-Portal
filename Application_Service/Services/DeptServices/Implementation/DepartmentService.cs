using Application_Service.Common;
using Application_Service.DTO_s.DeptDTO_s;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.DeptMappers;
using Application_Service.Services.DeptServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.DeptRepo;
using Domain_Service.RepoInterfaces.UnitOfWork;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Services.DeptServices.Implementation
{
    public class DepartmentService : IDepartmentService
    {
        private readonly IUnitOfWork _unitOfWork;
        public DepartmentService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }

        public async Task<ApiResponse<List<GetDepartmentDto>>> GetDepartmentsAsync()
        {
            try
            {
                var departments = await _unitOfWork.DepartmentRepository.GetAllAsync();

                if (departments == null || !departments.Any())
                {
                    return ApiResponse<List<GetDepartmentDto>>.Success(
                        new List<GetDepartmentDto>(),
                        "No departments found",
                        ResponseType.Ok);
                }

                var departmentDtos = departments.Map();

                return ApiResponse<List<GetDepartmentDto>>.Success(
                    departmentDtos,
                    "Departments retrieved successfully",
                    ResponseType.Ok);
            }
            catch (Exception )
            {

                return ApiResponse<List<GetDepartmentDto>>.Fail(
                    "Internal server error",
                    ResponseType.InternalServerError);
            }
        }
    }
}
