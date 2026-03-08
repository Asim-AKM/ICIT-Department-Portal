using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Entities.StudentModule;

namespace Application_Service.Mapper_s.StudentManagmenMappers
{
    public static class SessionMappers
    {
        public static List<SessionGetDTO> SessionsMapToSessionGetDto(this List<Session> sessions )
        {
           return  sessions.Select(session => new SessionGetDTO(
              session.SessionId,
              session.Name,
              session.StartDate,
              session.EndDate
          )).ToList();
        }
    }
}
