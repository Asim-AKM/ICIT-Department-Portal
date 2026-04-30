using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;

namespace Application_Service.Services.AdminServices.Interfaces
{
    public interface ISessionService
    {
            Task<ApiResponse<List<SessionGetDTO>>> GetActiveSessionsAsync();
            Task<SessionGetDTO> GetSessionByIdAsync(Guid sessionId);
            Task<ApiResponse<SessionAddDto>> CreateSessionAsync(SessionAddDto session);
            Task<SessionUpdateDto> UpdateSessionAsync(SessionUpdateDto updateDto);
            Task<bool> DeleteSessionAsync(Guid sessionId);
    }
}
