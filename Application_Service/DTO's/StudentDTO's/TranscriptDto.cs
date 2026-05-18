using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public class TranscriptDto
    {
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;

        public string Department { get; set; } = string.Empty;
        public string Program { get; set; } = string.Empty;
        public string Session { get; set; } = string.Empty;

        public double CGPA { get; set; }

        public int TotalEarnedCredits { get; set; }

        public int TotalRequiredCredits { get; set; }

        public double Percentage { get; set; }

        public List<TranscriptSemesterDto> Semesters { get; set; } = new();
    }
}
