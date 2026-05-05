using Application_Service.DTO_s.DeptDTO_s;
using Domain_Service.Entities.Academic;

namespace Application_Service.Mapper_s.DeptMappers
{
    public static class DepartmentMapper
    {
        public static List<GetDepartmentDto> Map(this List<Department> departments)
        {
            return departments.Select(department => new GetDepartmentDto
            (
                department.DepartmentId,
                department.Name,
                department.Description,
                department.Code,
                department.Status.ToString()

            )).ToList();
        }
    }
}
