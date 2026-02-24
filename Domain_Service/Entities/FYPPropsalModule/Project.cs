using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYPPropsalModule
{
    public class Project
    {
        [Key]
        public Guid ProjectId { get; set; }
        public String Title { get; set; } = string.Empty;
        public Guid FacultyId { get; set; } 
        public ICollection<Proposal> Proposals { get; set; } 
    }
}
