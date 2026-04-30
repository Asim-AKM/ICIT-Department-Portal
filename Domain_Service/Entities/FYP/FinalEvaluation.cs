using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FYP
{
    public class FinalEvaluation
    {
        [Key]
        public Guid EvaluationId { get; set; }
        public Guid ProposalId { get; set; }
        public int PresentationScore { get; set; }
        public int VivaScore { get; set; }
        public int ReportScore { get; set; }
        public int TotalScore { get; set; }
        public string Grade { get; set; } = string.Empty;
        public string Comments { get; set; } = string.Empty;
        public DateTime EvaluatedDate { get; set; }
    }
}
