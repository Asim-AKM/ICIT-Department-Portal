using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Academic
{
    public class Grade
    {
        [Key]
        public Guid GradeId { get; set; }
        public Guid EnrollmentId { get; set; }
        public string Grad { get; set; } = string.Empty; // A, B+, B, etc.
        public float GradePoints { get; set; }
        public int MidtermMarks { get; set; }
        public int FinalMarks { get; set; }
        public int AssignmentMarks { get; set; }
        public int QuizMarks { get; set; }
        public int TotalMarks { get; set; }
    }
}
