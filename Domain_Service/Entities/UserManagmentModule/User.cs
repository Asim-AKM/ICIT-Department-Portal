using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.UserManagmentModule
{
    public class User
    {
        public Guid UseriId { get; set; }
        public string FullName { get; set; }=string.Empty;
        public string UserName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Contact { get; set; }=string.Empty;
        public string ImageUrl { get; set; }=string.Empty;
        public UserStatus Status { get; set; } = UserStatus.Active;

        public DateTime  CreatedAt { get; set; }
    }
}
