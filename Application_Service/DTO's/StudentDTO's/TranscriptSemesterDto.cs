using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public class TranscriptSemesterDto
    {
        public string SemesterName { get; set; } = string.Empty;

        public string Season { get; set; } = string.Empty;   // Optional: Fall / Spring

        public int Year { get; set; }

        public double GPA { get; set; }

        public int TotalCredits { get; set; }

        public int EarnedCredits { get; set; }

        public List<TranscriptSubjectDto> Subjects { get; set; } = new();
    }
}
