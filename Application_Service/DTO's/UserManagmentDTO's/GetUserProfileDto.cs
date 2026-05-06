using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public class GetUserProfileDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public string UserName { get; set; }
        public string Email { get; set; }
        public string Contact { get; set; }
        public string CNIC { get; set; }
        public DateTime CreatedAt { get; set; }
        public RoleType Role { get; set; }
        public string Department { get; set; }
        public string ImageUrl { get; set; }
    }
}
