using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Entities.StudentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Mapper_s.StudentManagmenMappers
{
    public static class StudentManagmentMappers
    {
        public static Session MapToSession(this SessionAddDto dto)
        {
            return new Session
            {
              SessionId = new Guid(),
                Name = dto.Name,
                StartYear = dto.StartYear,
                EndYear = dto.EndYear
            };
        }
    }
}
