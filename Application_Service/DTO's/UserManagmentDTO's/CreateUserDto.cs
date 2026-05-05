using Domain_Service.Enum;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public record CreateUserDto(Guid? DepartmentId,string FullName, string UserName, string Email, string CNIC, string Password,  RoleType Role,bool GeneratTempPassword,bool SendWelcomeEmail);
}
