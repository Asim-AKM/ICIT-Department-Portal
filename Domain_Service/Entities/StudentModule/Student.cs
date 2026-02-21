using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.StudentModule
{
    public class Student
    {
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string RegistrationNumber { get; set; } = string.Empty;
        public string RollNumber { get; set; } = string.Empty;
        public Guid SamesterId { get; set; }
        public Decimal GPA { get; set; }
        public ICollection<FeeRecord> FeeRecords { get; set; }
    }
}
