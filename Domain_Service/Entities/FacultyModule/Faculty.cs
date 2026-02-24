using Domain_Service.Entities.FYPPropsalModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FacultyModule
{
    
    public class Faculty
    {
        [Key]
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public String Department { get; set; } = string.Empty;
        public List<Subject> SubjectsTaught { get; set; } = new List<Subject>();
        public List<Project> SupervisedProjects { get; set; } = new List<Project>();
    }
}
