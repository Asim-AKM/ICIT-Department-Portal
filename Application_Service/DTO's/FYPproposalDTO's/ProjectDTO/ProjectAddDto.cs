using Domain_Service.Entities.FYPPropsalModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FYPproposalDTO_s.ProjectDTO
{
public record  ProjectAddDto(Guid ProjectId, String Title, Guid FacultyId, ICollection<Proposal> Proposals);
}
