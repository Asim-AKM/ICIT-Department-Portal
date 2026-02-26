using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.Enum;

namespace Application_Service.Mapper_s.UserManagmentMappers
{
    public static class UserRoleMapper
    {
        public static UserRole MapToUserRoleDomain(this User user, RoleType role)
        {
                return new UserRole
                {
                    UserRoleId = Guid.NewGuid(),
                    UserId = user.UserId,
                    RoleName = role
                };
        }
    }
}
