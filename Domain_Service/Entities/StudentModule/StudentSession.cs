using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.StudentModule
{
    public class StudentSession
    {
        [Key]
        public Guid SessionId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g. "Session 2026-2029"
        public DateTime StartYear { get; set; }
        public DateTime EndYear { get; set; }
        public List<Semester> Semesters { get; set; } = new List<Semester>();
    }
}
