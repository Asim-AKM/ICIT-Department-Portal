using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace Domain_Service.Entities.StudentModule
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g. "Session 2026-2029"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }

        public List<Semester> Semesters { get; set; } = new();
    }
}
