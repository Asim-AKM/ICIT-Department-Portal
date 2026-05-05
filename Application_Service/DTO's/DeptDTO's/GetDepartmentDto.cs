using Domain_Service.Enum;

namespace Application_Service.DTO_s.DeptDTO_s
{
    public record GetDepartmentDto(Guid DepartmentId, string Name, string Code, string Description, string Status);
    
}
