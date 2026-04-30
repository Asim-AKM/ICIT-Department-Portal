using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.FYP
{
    public class ProposalTeamMember
    {
        [Key]
        public Guid TeamMemberId { get; set; }
        public Guid ProposalId { get; set; }
        public Guid StudentId { get; set; }
        public TeamMemberRole TeamMemberRole { get; set; } // Leader, Member
    }
}
