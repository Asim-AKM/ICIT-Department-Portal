using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public class UserProfileImageUploadDto
    {
        public Guid UserId { get; set; }
        public IFormFile file { get; set; }

    }
}
