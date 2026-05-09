using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public record UpdateUserDto
  (
      Guid UserId,
      string FullName,
      string UserName,
      string Contact,
      string CNIC,
      string Email,
      RoleType Role,
      Guid? DepartmentId,
      UserStatus Status
  );
}
