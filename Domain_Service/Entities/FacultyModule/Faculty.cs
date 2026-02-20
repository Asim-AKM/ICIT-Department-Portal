using Domain_Service.Entities.FYPPropsalModule;
using System;
using System.Collections;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FacultyModule
{
    public class Faculty
    {
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public String Department { get; set; } = string.Empty;
        public ICollection<Subject> SubjectsTaught { get; set; }
        public ICollection<Project> SupervisedProjects { get; set; }
    }
}
