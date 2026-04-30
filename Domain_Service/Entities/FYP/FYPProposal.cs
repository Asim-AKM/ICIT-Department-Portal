using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYP
{
    public class FYPProposal
    {
        [Key]
        public Guid ProposalId { get; set; }
        public string Title { get; set; } = string.Empty;
        public string Description { get; set; } = string.Empty;
        public string Category { get; set; } = string.Empty;
        public string Technologies { get; set; } = string.Empty; // Comma-separated
        public Guid StudentId { get; set; }
        public Guid? SupervisorId { get; set; } // FacultyId
        public DateTime SubmittedDate { get; set; }
        public ProposalStatus Status { get; set; } // Pending, Approved, Rejected, Revision
        public bool IsLocked { get; set; }
        public DateTime? LockUntil { get; set; }
        public string ReviewComments { get; set; } = string.Empty;
        public int? EvaluationScore { get; set; }
    }

}
