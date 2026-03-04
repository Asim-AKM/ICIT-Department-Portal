using Domain_Service.Entities.FacultyModule;
using Domain_Service.Entities.FYPPropsalModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FacultyModelDTO_s.FacultyDTO
{
public  record  FacultyUpdateDto(Guid FacultyId, Guid UserId, String Department, List<Subject> SubjectsTaught, List<Project> SupervisedProjects);

}
