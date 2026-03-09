using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.StudentModels
{
    public class BulkVerifyRequest
    {
        public List<Guid> StudentIds { get; set; } = new List<Guid>();
        public StudentStatus Status { get; set; }
    }
}
