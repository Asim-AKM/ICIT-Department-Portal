using Microsoft.AspNetCore.Http;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public class UploadBulkStudentDto
    {
        public Guid sessionId { get; set; }
        public Guid DepartmentId { get; set; }
    }
}
