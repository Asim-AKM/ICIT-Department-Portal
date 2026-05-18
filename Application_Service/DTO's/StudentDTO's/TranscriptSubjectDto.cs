using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public class TranscriptSubjectDto
    {
        public string Title { get; set; } = string.Empty;

        public int CreditHours { get; set; }

        public string Grade { get; set; } = string.Empty;

        public float GradePoints { get; set; }
    }
}
