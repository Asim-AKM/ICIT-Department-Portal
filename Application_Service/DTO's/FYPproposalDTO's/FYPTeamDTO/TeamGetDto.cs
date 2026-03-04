using Domain_Service.Entities.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FYPproposalDTO_s.FYPTeamDTO
{
public record  TeamGetDto(Guid TeamId, string TeamName, Guid LeaderId, ICollection<Student> Members, Guid FacultyId);

}
