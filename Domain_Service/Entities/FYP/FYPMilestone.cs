using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYP
{
    public class FYPMilestone
    {
        [Key]
        public Guid MilestoneId { get; set; }
        public Guid ProposalId { get; set; }
        public string Name { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
        public DateTime? SubmittedDate { get; set; }
        public MilestoneStatus Status { get; set; }
        public string Feedback { get; set; } = string.Empty;
    }
}
