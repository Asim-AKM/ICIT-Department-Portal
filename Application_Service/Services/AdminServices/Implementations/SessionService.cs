using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.StudentManagmenMappers;
using Application_Service.Services.AdminServices.Interfaces;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;

namespace Application_Service.Services.AdminServices.Implementations
{
    public class SessionService : ISessionService
    {
        private readonly IUnitOfWork _unitOfWork;
        public SessionService(IUnitOfWork unitOfWork)
        {
            _unitOfWork = unitOfWork;
        }
        public async Task<ApiResponse<SessionAddDto>> CreateSessionAsync(SessionAddDto request)
        {
            var domain = request.MapToSession();
            await _unitOfWork.SessionRepo.CreateAsync(domain);
            await _unitOfWork.SaveChangesAsync();
            return ApiResponse<SessionAddDto>.Success(request, "Session Created Successfully", ResponseType.Created);
        }

        public Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<List<SessionGetDTO>> GetAllSessionsAsync()
        {
            throw new NotImplementedException();
        }

        public Task<SessionGetDTO> GetSessionByIdAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public Task<SessionUpdateDto> UpdateSessionAsync(SessionUpdateDto updateDto)
        {
            throw new NotImplementedException();
        }
    }
}
