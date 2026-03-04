using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s.SemesterDTO_s
{
public record  SemesterGetDto(Guid SemesterId, Guid SessionId, string Name, int Year, DateTime StartDate, DateTime EndDate, List<Student> Students);

}
