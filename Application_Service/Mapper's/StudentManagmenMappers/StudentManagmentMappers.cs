using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Entities.StudentModule;

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
                StartDate = dto.StartDate,
                EndDate = dto.EndDate
            };
        }
    }
}
