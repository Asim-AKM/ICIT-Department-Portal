using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.StudentManagmenMappers;
using Application_Service.Services.AdminServices.Interfaces;
using Domain_Service.Entities.Academic;
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

                // Validate: Sirf year check (months/days ignore)
                if (request.EndDate.Year != calculatedEndDate.Year)
                {
                    return ApiResponse<SessionAddDto>.Fail(
                        request,
                        $"Session must be exactly 4 years long. Start year {request.StartDate.Year} should end in year {calculatedEndDate.Year} (not {request.EndDate.Year}).",
                        ResponseType.BadRequest);
                }

                // Check 1: Same name already exists?
                var sameName = await _unitOfWork.SessionRepo.FirstOrDefaultAsync(s =>
                    s.Name == request.Name);

                if (sameName != null)
                {
                    return ApiResponse<SessionAddDto>.Fail(
                        request,
                        $"A session with name '{request.Name}' already exists. Please use a different name.",
                        ResponseType.Conflict);
                }

                // Check 2: Same starting year already exists?
                var existingWithSameStartYear = await _unitOfWork.SessionRepo.FirstOrDefaultAsync(s =>
                    s.StartDate.Year == request.StartDate.Year);

                if (existingWithSameStartYear != null)
                {
                    return ApiResponse<SessionAddDto>.Fail(
                        request,
                        $"Year {request.StartDate.Year} already has a session: '{existingWithSameStartYear.Name}'",
                        ResponseType.Conflict);
                }

                // No overlap check - batches can overlap

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


        public async Task<ApiResponse<bool>> UpdateSessionStatus(Guid sessionId, SessionStatus newStatus)
        {
            try
            {
                var session = await _unitOfWork.SessionRepo.FirstOrDefaultAsync(s => s.SessionId == sessionId);

                if (session == null)
                {
                    return ApiResponse<bool>.Fail(
                        false,
                        "Session not found",
                        ResponseType.NotFound);
                }

                if (session.Status == newStatus)
                {
                    string statusName = newStatus == SessionStatus.Active ? "Active" :
                                        newStatus == SessionStatus.Inactive ? "Inactive" : "Completed";

                    return ApiResponse<bool>.Fail(
                        false,
                        $"Session is already {statusName}",
                        ResponseType.Conflict);
                }

                // Validation: Can only mark as Completed if current status is Inactive
                if (newStatus == SessionStatus.Completed && session.Status != SessionStatus.Inactive)
                {
                    return ApiResponse<bool>.Fail(
                        false,
                        "Session must be Inactive before marking as Completed",
                        ResponseType.BadRequest);
                }

                session.Status = newStatus;
                await _unitOfWork.SessionRepo.Update(session);
                await _unitOfWork.SaveChangesAsync();

                string successMessage = newStatus == SessionStatus.Active ? "activated" :
                                        newStatus == SessionStatus.Inactive ? "deactivated" : "completed";

                return ApiResponse<bool>.Success(
                    true,
                    $"Session '{session.Name}' has been {successMessage} successfully",
                    ResponseType.Ok);
            }
            catch (Exception ex)
            {
                return ApiResponse<bool>.Fail(
                    false,
                    $"Failed to update session status: {ex.Message}",
                    ResponseType.BadRequest);
            }
        }
        public Task<bool> DeleteSessionAsync(Guid sessionId)
        {
            throw new NotImplementedException();
        }


        public async Task<ApiResponse<List<SessionGetDTO>>> GetActiveSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepo.GetSessionsByStatusAsync(SessionStatus.Active);

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



        public async Task<ApiResponse<List<SessionGetDTO>>> GetAllSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepo.GetSessionsByStatusAsync();

            if (!sessions.Any())
            {
                return ApiResponse<List<SessionGetDTO>>.Fail(
                    "No sessions found",
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

        public async Task<ApiResponse<List<SessionGetDTO>>> GetCompleteSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepo.GetSessionsByStatusAsync(SessionStatus.Completed);

            if (!sessions.Any())
            {
                return ApiResponse<List<SessionGetDTO>>.Fail(
                    "No Complete sessions found",
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

        public async Task<ApiResponse<List<SessionGetDTO>>> GetInActiveSessionsAsync()
        {
            var sessions = await _unitOfWork.SessionRepo.GetSessionsByStatusAsync(SessionStatus.Inactive);

            if (!sessions.Any())
            {
                return ApiResponse<List<SessionGetDTO>>.Fail(
                    "No InActive sessions found",
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
