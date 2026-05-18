using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.SubjectDTO_s
{
    public class GetSubjectByDprtIdAndSemesterIdDTO
    {
        public Guid DepartmentId { get; set; }
        public Guid SemesterId { get; set; }
    }
}
