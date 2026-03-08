using Domain_Service.Enum;

namespace Application_Service.DTO_s.StudentDTO_s
{
    public record SessionGetDTO(Guid SessionId, string Name, DateTime StartYear, DateTime EndYear,SessionStatus SessionStatus);
    
}
