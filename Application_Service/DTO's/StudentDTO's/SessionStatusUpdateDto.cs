using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public class SessionStatusUpdateDto
    {
        public Guid SessionId { get; set; }
        public SessionStatus Status { get; set; }
    }
}
