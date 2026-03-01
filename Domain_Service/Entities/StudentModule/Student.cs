using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.StudentModule
{
    public class Student
    {
        [Key]
        public Guid StudentId { get; set; }
        public Guid UserId { get; set; }
        public string RegistrationNo { get; set; } = string.Empty;
        public string RollNo { get; set; } = string.Empty;
        public Guid SamesterId { get; set; }
        public Guid SessionId { get; set; }
        public float GPA { get; set; }
        public List<FeeRecord> FeeRecords { get; set; } = new List<FeeRecord>();
    }
}
