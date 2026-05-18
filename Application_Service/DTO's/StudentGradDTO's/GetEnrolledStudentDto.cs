using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentGradDTO_s
{
    public class GetEnrolledStudentDto
    {
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public string StudentName { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public Guid? GradeId { get; set; }
        public string? Grade { get; set; }
        public float? GradePoints { get; set; }
        public int? MidtermMarks { get; set; }
        public int? FinalMarks { get; set; }
        public int? AssignmentMarks { get; set; }
        public int? QuizMarks { get; set; }
        public int? TotalMarks { get; set; }
    }
}
