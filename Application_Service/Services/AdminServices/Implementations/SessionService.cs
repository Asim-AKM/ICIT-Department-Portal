using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.StudentManagmenMappers;
using Application_Service.Services.AdminServices.Interfaces;
using Domain_Service.Entities.StudentModule;
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
            try
            {
                DateTime calculatedEndDate = request.StartDate.AddYears(4).AddDays(-1);

                var overlapping = await _unitOfWork.SessionRepo.FirstOrDefaultAsync(s => request.StartDate <= s.EndDate && calculatedEndDate >= s.StartDate);

                if (overlapping != null)
                {
                    return ApiResponse<SessionAddDto>.Fail(
                        request,
                        "Session dates overlap with an existing session.",
                        ResponseType.Conflict);
                }

                await _unitOfWork.ExecuteInTransactionAsync(async () =>
                {
                    var session = request.MapToSession();
                    session.EndDate = calculatedEndDate;

                    await _unitOfWork.SessionRepo.CreateAsync(session);

                    DateTime semesterStart = session.StartDate;

                    for (int i = 1; i <= 8; i++)
                    {
                        DateTime semesterEnd = semesterStart.AddMonths(6).AddDays(-1);
                        int academicYear = ((i - 1) / 2) + 1;

                        await _unitOfWork.SemesterRepo.CreateAsync(new Semester
                        {
                            SemesterId = Guid.NewGuid(),
                            SessionId = session.SessionId,
                            Name = $"Semester {i}",
                            Order = i,
                            AcademicYear = academicYear,
                            StartDate = semesterStart,
                            EndDate = semesterEnd
                        });

                        semesterStart = semesterStart.AddMonths(6);
                    }
                });

                return ApiResponse<SessionAddDto>.Success(
                    request,
                    "Session created successfully",
                    ResponseType.Created);
            }
            catch (Exception ex)
            {
                return ApiResponse<SessionAddDto>.Fail(
                    request,
                    $"Failed to create session: {ex.Message}",
                    ResponseType.BadRequest);
            }
        }

        public Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }

        public async Task<ApiResponse<List<SessionGetDTO>>> GetActiveSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepo.GetActiveSessionsAsync();

            if (!sessions.Any())
            {
                return ApiResponse<List<SessionGetDTO>>.Fail(
                    "No active sessions found",
                    ResponseType.NotFound
                );
            }

            var result = sessions.SessionsMapToSessionGetDto();

            return ApiResponse<List<SessionGetDTO>>.Success(
                result,
                "Sessions fetched successfully",
                ResponseType.Ok
            );
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
