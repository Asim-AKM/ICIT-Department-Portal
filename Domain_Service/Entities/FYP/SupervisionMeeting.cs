using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYP
{
    public class SupervisionMeeting
    {
        [Key]
        public Guid MeetingId { get; set; }
        public Guid ProposalId { get; set; }
        public DateTime MeetingDate { get; set; }
        public string Duration { get; set; } = string.Empty;
        public string Agenda { get; set; } = string.Empty;
        public string Discussion { get; set; } = string.Empty;
        public string ActionItems { get; set; } = string.Empty;
        public DateTime NextMeetingDate { get; set; }
    }

}
