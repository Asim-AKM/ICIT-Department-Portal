using Domain_Service.Enum;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public record CreateUserDto(string FullName, string UserName, string Email, string Password, RoleType Role);
}
