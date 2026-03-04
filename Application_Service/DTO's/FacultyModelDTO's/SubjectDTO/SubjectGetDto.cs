using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FacultyModelDTO_s.SubjectDTO
{
public record  SubjectGetDto(Guid SubjectId, String Title, Guid SemesterId, Guid FacultyId);

}
