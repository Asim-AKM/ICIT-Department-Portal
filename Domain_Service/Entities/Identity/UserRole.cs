using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Identity
{
    public class UserRole
    {
        [Key]
        public Guid  UserRoleId { get; set; }
        public Guid  UserId { get; set; }
        public RoleType RoleName { get; set; }
    }
}
