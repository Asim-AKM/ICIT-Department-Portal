using Domain_Service.Entities.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s.Student
{
    public record StudentUpdateDto(Guid StudentId, Guid UserId, string RegistrationNo, string RollNo, Guid SamesterId, Guid SessionId, float GPA, List<FeeRecord> FeeRecords);
}
