using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.UserManagmentModule
{
    public class UserRole
    {
        [Key]
        public Guid  UserRoleId { get; set; }
        public Guid  UserId { get; set; }
        public RoleType RoleName { get; set; }
    }
}
