using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FYPproposalDTO_s.ProposalDTO
{
public record  ProposalGetDto(Guid ProposalId, Guid TeamId, String Title, String Description, String Status, String SubmissionDate, bool Locked);

}
