using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Academic
{
    public class Session
    {
        [Key]
        public Guid SessionId { get; set; }
        public string Name { get; set; } = string.Empty; // e.g. "Session 2026-2029"
        public DateTime StartDate { get; set; }
        public DateTime EndDate { get; set; }
        public SessionStatus Status { get; set; }

        public List<Semester> Semesters { get; set; } = new();
    }
}
