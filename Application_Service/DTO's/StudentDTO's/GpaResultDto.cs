using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public record GpaResultDto(
      Guid StudentId,
      Guid SemesterId,
      double GPA,
      int TotalCreditHours
  );

    public record CgpaResultDto(
    Guid StudentId,
    double CGPA,
    int TotalCreditHours
);
}
