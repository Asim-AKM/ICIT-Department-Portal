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
        Rejected = 3,
        Unvarified = 4,
        Varified = 5
     
    }

    public enum SessionStatus
    {
        Active =1, // When session Currently Running
        Inactive = 2, //Incase We Inactive current Session 
        Completed = 3, // when Session Completed
    }
}
