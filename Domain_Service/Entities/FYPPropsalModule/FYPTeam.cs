using Domain_Service.Entities.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYPPropsalModule
{
    public class FYPTeam
    {
        public Guid TeamId { get; set; }
        public string TeamName { get; set; } = string.Empty;
        public Guid LeaderId { get; set; }
        public ICollection<Student> Members { get; set; }
        public Guid FacultyId { get; set; }

    }
}
