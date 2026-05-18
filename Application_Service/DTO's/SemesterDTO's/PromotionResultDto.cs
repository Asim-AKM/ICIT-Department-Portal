using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.SemesterDTO_s
{
    public class PromotionResultDto
    {
        public Guid StudentId { get; set; }
        public Guid CurrentSemesterId { get; set; }
        public Guid? NextSemesterId { get; set; }
        public double GPA { get; set; }
        public double CGPA { get; set; }
        public bool IsPromoted { get; set; }
        public bool IsOnProbation { get; set; }
        public int FailedSubjects { get; set; }
        public string Status { get; set; } = string.Empty; // Promoted / Repeat / Probation
    }
}
