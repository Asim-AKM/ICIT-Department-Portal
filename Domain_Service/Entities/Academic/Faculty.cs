using Domain_Service.Entities.FYP;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Academic
{
    
    public class Faculty
    {
        [Key]
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Designation { get; set; } = string.Empty; // Professor, Assistant Professor, etc.
        public DateTime JoiningDate { get; set; }
        public List<Subject> SubjectsTaught { get; set; } = new List<Subject>();
        public List<FYPProposal> SupervisedProjects { get; set; } = new List<FYPProposal>();
    }
}
