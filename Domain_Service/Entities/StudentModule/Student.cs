using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.StudentModule
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public Guid SamesterId { get; set; }
        public float GPA { get; set; }
        public List<FeeRecord> FeeRecords { get; set; } = new List<FeeRecord>();
    }
}
