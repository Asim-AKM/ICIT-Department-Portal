using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.StudentModels
{
    public class LockResultRequest
    {
        public Guid SemesterId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Remarks { get; set; } = string.Empty;
    }
}
