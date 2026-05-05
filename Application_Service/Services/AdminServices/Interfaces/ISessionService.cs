using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Enum;

namespace Application_Service.Services.AdminServices.Interfaces
{
    public interface ISessionService
    {
        Task<ApiResponse<List<SessionGetDTO>>> GetAllSessionsAsync();
        Task<ApiResponse<List<SessionGetDTO>>> GetActiveSessionsAsync();
        Task<ApiResponse<List<SessionGetDTO>>> GetInActiveSessionsAsync();
        Task<ApiResponse<List<SessionGetDTO>>> GetCompleteSessionsAsync();
        Task<SessionGetDTO> GetSessionByIdAsync(Guid sessionId);
        Task<ApiResponse<SessionAddDto>> CreateSessionAsync(SessionAddDto session);
        Task<SessionUpdateDto> UpdateSessionAsync(SessionUpdateDto updateDto);
        Task<ApiResponse<bool>> UpdateSessionStatus(Guid sessionId, SessionStatus newStatus);
        Task<bool> DeleteSessionAsync(Guid sessionId);
    }
}
