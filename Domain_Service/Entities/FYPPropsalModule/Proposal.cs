using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYPPropsalModule
{
    public class Proposal
    {
        public Guid ProposalId { get; set; }
        public Guid TeamId { get; set; }
        public String Title { get; set; } = string.Empty;
        public String Description { get; set; } = string.Empty;
        public String Status { get; set; } = string.Empty;
        public String SubmissionDate { get; set; } 
        public bool Locked { get; set; } 
    }
}
