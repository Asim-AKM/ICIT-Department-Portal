using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Enum
{
    public enum UserStatus
    {
        Active = 1,
        Inactive = 2,
        Blocked = 3,
        Suspended = 4
    }

    public enum StudentStatus
    {
        Pending = 1,
        Approved = 2,
        Rejected = 3
    }
}
