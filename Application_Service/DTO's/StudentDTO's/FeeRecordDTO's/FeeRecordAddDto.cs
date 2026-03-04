using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s.FeeRecordDTO_s
{
 public record  FeeRecordAddDto(Guid FeeId, Guid StudentId, Guid SemesterId, float Amount, string Status, DateTime DueDate);


}
