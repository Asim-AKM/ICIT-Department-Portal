using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.AuthenticationModels
{
    public class GetUserByRoleAndStatusRequest
    {
        public RoleType? role { get; set; }
        public UserStatus? status { get; set; }
        public int pageNumber { get; set; } = 1;
        public int pageSize { get; set; } = 10;
    }
}
