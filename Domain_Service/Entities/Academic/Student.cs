using Domain_Service.Entities.Finance;
using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Academic
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public string StudentName { get; set; }= string.Empty;
        public string StudentEmail { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public string RegistrationNo { get; set; } = string.Empty;
        public string CNIC {  get; set; } = string.Empty;
        public float GPA { get; set; } = float.MinValue;
        public Guid SamesterId { get; set; }
        public Guid SessionId { get; set; }
        public StudentStatus Status { get; set; }
        public List<FeeRecord> FeeRecords { get; set; } = new List<FeeRecord>();
    }
}
