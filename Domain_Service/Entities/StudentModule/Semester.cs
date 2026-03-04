using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.StudentModule
{
    public class Semester
    {
        [Key]
        public Guid SemesterId { get; set; }
        public Guid SessionId { get; set; }      // Foreign key to StudentSession
        public string Name { get; set; } = string.Empty;   // e.g. "1st Semester"
        public int Year { get; set; }                      // e.g. 2025
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        // Navigation property
        public List<Student> Students { get; set; } = new List<Student>();
    }

}
