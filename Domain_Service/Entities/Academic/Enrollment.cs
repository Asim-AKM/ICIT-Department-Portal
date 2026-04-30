using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Academic
{
    public class Enrollment
    {
        [Key]
        public Guid EnrollmentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid SubjectId { get; set; }
        public Guid SemesterId { get; set; }
        public EnrollmentStatus Status { get; set; }
    }
}
