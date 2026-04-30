using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Feedback
{
    public class StudentFeedback
    {
        [Key]
        public Guid FeedbackId { get; set; }
        public Guid FacultyId { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public string Remarks { get; set; } = string.Empty;
        public int PerformanceRating { get; set; } // 1-5
        public DateTime FeedbackDate { get; set; }
    }
}
