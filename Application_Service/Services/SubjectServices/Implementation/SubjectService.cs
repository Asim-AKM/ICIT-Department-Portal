using Application_Service.Common;
using Application_Service.DTO_s.SubjectDTO_s;
using Application_Service.RequestAndResponseModel.Pagination;
using Application_Service.RequestAndResponseModel.SubjectManagmengModels;
using Application_Service.Services.SubjectServices.Interfaces;
using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.SubjectServices.Implementation
{
    public class SubjectService : ISubjectService
    {
        IUnitOfWork _uow;
        public SubjectService(IUnitOfWork unitOfWork)
        {
            _uow = unitOfWork;
        }
        public async Task<ApiResponse<string>> CreateSubject(CreateSubjectRequest request)
        {
            try
            {
                // 🔹 Validate Title
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return ApiResponse<string>.Fail(
                        "Subject title is required",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate DepartmentId
                if (request.DepartmentId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid department identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate SemesterId
                if (request.SemesterId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid semester identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate Department Exists
                var departmentExists = await _uow.DepartmentRepository.Query()
                    .AnyAsync(d => d.DepartmentId == request.DepartmentId);

                if (!departmentExists)
                {
                    return ApiResponse<string>.Fail(
                        "Department not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Semester Exists
                var semesterExists = await _uow.SemesterRepo.Query()
                    .AnyAsync(s => s.SemesterId == request.SemesterId);

                if (!semesterExists)
                {
                    return ApiResponse<string>.Fail(
                        "Semester not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Faculty (optional)
                if (request.FacultyId.HasValue)
                {
                    var facultyExists = await _uow.FucaltyRepo.Query()
                        .AnyAsync(f => f.FacultyId == request.FacultyId.Value);

                    if (!facultyExists)
                    {
                        return ApiResponse<string>.Fail(
                            "Faculty not found",
                            ResponseType.NotFound);
                    }
                }

                // 🔹 Duplicate Check
                // Same Title is allowed in different departments/semesters.
                // Duplicate is only blocked within the same Department + Semester.
                var normalizedTitle = request.Title.Trim().ToLower();

                var subjectExists = await _uow.SubjectRepository.Query()
                    .AnyAsync(s =>
                        s.DepartmentId == request.DepartmentId &&
                        s.SemesterId == request.SemesterId &&
                        s.Title.Trim().ToLower() == normalizedTitle &&
                        s.IsActive);

                if (subjectExists)
                {
                    return ApiResponse<string>.Fail(
                        "Subject already exists in this department and semester",
                        ResponseType.BadRequest);
                }

                // 🔹 Create Subject
                var subject = new Subject
                {
                    SubjectId = Guid.NewGuid(),
                    Title = request.Title.Trim(),
                    DepartmentId = request.DepartmentId,
                    SemesterId = request.SemesterId,
                    FacultyId = request.FacultyId,
                    CreditHours = request.CreditHours,
                    IsActive = true
                };

                // 🔹 Save
                await _uow.SubjectRepository.CreateAsync(subject);
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success("Creation Info",
                    "Subject created successfully",
                    ResponseType.Created);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to create subject",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<PaginationResponse<GetSubjectDto>>> GetAllSubject(int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // 🔹 Normalize Pagination
                if (pageNumber <= 0)
                    pageNumber = 1;

                if (pageSize <= 0)
                    pageSize = 10;

                // 🔹 Base Query (only active subjects)
                var query = _uow.SubjectRepository.Query()
                    .Where(s => s.IsActive);

                // 🔹 Total Records
                var totalRecords = await query.CountAsync();

                // 🔹 Handle Empty Result
                if (totalRecords == 0)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Success(
                        new PaginationResponse<GetSubjectDto>(),
                        "No subjects found",
                        ResponseType.Ok);
                }

                // 🔹 Load Related Data Dictionaries
                var departments = await _uow.DepartmentRepository.Query()
                    .Select(d => new { d.DepartmentId, d.Name })
                    .ToListAsync();

                var semesters = await _uow.SemesterRepo.Query()
                    .Select(s => new { s.SemesterId, s.Name })
                    .ToListAsync();

                var faculties = await _uow.FucaltyRepo.Query()
                    .Select(f => new { f.FacultyId, f.UserId })
                    .ToListAsync();

                var users = await _uow.UserRepo.Query()
                    .Select(u => new { u.UserId, u.FullName })
                    .ToListAsync();

                var departmentDict = departments.ToDictionary(x => x.DepartmentId, x => x.Name);
                var semesterDict = semesters.ToDictionary(x => x.SemesterId, x => x.Name);
                var facultyDict = faculties.ToDictionary(x => x.FacultyId, x => x.UserId);
                var userDict = users.ToDictionary(x => x.UserId, x => x.FullName);

                // 🔹 Get Paged Data
                var subjects = await query
                    .OrderBy(s => s.Title)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // 🔹 Map DTO
                var result = subjects.Select(subject =>
                {
                    string facultyName = string.Empty;

                    if (subject.FacultyId.HasValue &&
                        facultyDict.ContainsKey(subject.FacultyId.Value))
                    {
                        var facultyUserId = facultyDict[subject.FacultyId.Value];

                        if (userDict.ContainsKey(facultyUserId))
                        {
                            facultyName = userDict[facultyUserId];
                        }
                    }

                    return new GetSubjectDto
                    {
                        SubjectId = subject.SubjectId,
                        Title = subject.Title,
                        DepartmentId = subject.DepartmentId,
                        DepartmentName = departmentDict.ContainsKey(subject.DepartmentId)
                            ? departmentDict[subject.DepartmentId]
                            : string.Empty,
                        SemesterId = subject.SemesterId,
                        SemesterName = semesterDict.ContainsKey(subject.SemesterId)
                            ? semesterDict[subject.SemesterId]
                            : string.Empty,
                        FacultyId = subject.FacultyId,
                        FacultyName = facultyName,
                        CreditHours = subject.CreditHours,
                        IsActive = subject.IsActive
                    };
                }).ToList();

                // 🔹 Pagination Response
                var response = new PaginationResponse<GetSubjectDto>
                {
                    Items = result,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };

                return ApiResponse<PaginationResponse<GetSubjectDto>>.Success(
                    response,
                    "Subjects retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                    "Failed to retrieve subjects",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<PaginationResponse<GetSubjectDto>>> GetSubjectsByDepartmentAndSemester(Guid DepartmentId, Guid SemesterId, int pageNumber = 1, int pageSize = 10)
        {
            try
            {
                // 🔹 Validate DepartmentId
                if (DepartmentId == Guid.Empty)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                        "Invalid department identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate SemesterId
                if (SemesterId == Guid.Empty)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                        "Invalid semester identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Normalize Pagination
                if (pageNumber <= 0)
                    pageNumber = 1;

                if (pageSize <= 0)
                    pageSize = 10;

                // 🔹 Validate Department Exists
                var departmentExists = await _uow.DepartmentRepository.Query()
                    .AnyAsync(d => d.DepartmentId == DepartmentId);

                if (!departmentExists)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                        "Department not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Semester Exists
                var semesterExists = await _uow.SemesterRepo.Query()
                    .AnyAsync(s => s.SemesterId == SemesterId);

                if (!semesterExists)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                        "Semester not found",
                        ResponseType.NotFound);
                }

                // 🔹 Base Query
                var query = _uow.SubjectRepository.Query()
                    .Where(s =>
                        s.IsActive &&
                        s.DepartmentId == DepartmentId &&
                        s.SemesterId == SemesterId);

                // 🔹 Total Records
                var totalRecords = await query.CountAsync();

                // 🔹 Handle Empty Result
                if (totalRecords == 0)
                {
                    return ApiResponse<PaginationResponse<GetSubjectDto>>.Success(
                        new PaginationResponse<GetSubjectDto>(),
                        "No subjects found",
                        ResponseType.Ok);
                }

                // 🔹 Load Related Data Dictionaries
                var departments = await _uow.DepartmentRepository.Query()
                    .Select(d => new { d.DepartmentId, d.Name })
                    .ToListAsync();

                var semesters = await _uow.SemesterRepo.Query()
                    .Select(s => new { s.SemesterId, s.Name })
                    .ToListAsync();

                var faculties = await _uow.FucaltyRepo.Query()
                    .Select(f => new { f.FacultyId, f.UserId })
                    .ToListAsync();

                var users = await _uow.UserRepo.Query()
                    .Select(u => new { u.UserId, u.FullName })
                    .ToListAsync();

                var departmentDict = departments.ToDictionary(x => x.DepartmentId, x => x.Name);
                var semesterDict = semesters.ToDictionary(x => x.SemesterId, x => x.Name);
                var facultyDict = faculties.ToDictionary(x => x.FacultyId, x => x.UserId);
                var userDict = users.ToDictionary(x => x.UserId, x => x.FullName);

                // 🔹 Get Paged Data
                var subjects = await query
                    .OrderBy(s => s.Title)
                    .Skip((pageNumber - 1) * pageSize)
                    .Take(pageSize)
                    .ToListAsync();

                // 🔹 Map DTO
                var result = subjects.Select(subject =>
                {
                    string facultyName = string.Empty;

                    if (subject.FacultyId.HasValue &&
                        facultyDict.ContainsKey(subject.FacultyId.Value))
                    {
                        var facultyUserId = facultyDict[subject.FacultyId.Value];

                        if (userDict.ContainsKey(facultyUserId))
                        {
                            facultyName = userDict[facultyUserId];
                        }
                    }

                    return new GetSubjectDto
                    {
                        SubjectId = subject.SubjectId,
                        Title = subject.Title,
                        DepartmentId = subject.DepartmentId,
                        DepartmentName = departmentDict.ContainsKey(subject.DepartmentId)
                            ? departmentDict[subject.DepartmentId]
                            : string.Empty,
                        SemesterId = subject.SemesterId,
                        SemesterName = semesterDict.ContainsKey(subject.SemesterId)
                            ? semesterDict[subject.SemesterId]
                            : string.Empty,
                        FacultyId = subject.FacultyId,
                        FacultyName = facultyName,
                        CreditHours = subject.CreditHours,
                        IsActive = subject.IsActive
                    };
                }).ToList();

                // 🔹 Pagination Response
                var response = new PaginationResponse<GetSubjectDto>
                {
                    Items = result,
                    PageNumber = pageNumber,
                    PageSize = pageSize,
                    TotalRecords = totalRecords
                };

                return ApiResponse<PaginationResponse<GetSubjectDto>>.Success(
                    response,
                    "Subjects retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<PaginationResponse<GetSubjectDto>>.Fail(
                    "Failed to retrieve subjects",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<string>> DeleteSubject(Guid subjectId)
        {
            try
            {
                // 🔹 Validate SubjectId
                if (subjectId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid subject identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Get Subject
                var subject = await _uow.SubjectRepository.GetByIdAsync(subjectId);

                if (subject == null)
                {
                    return ApiResponse<string>.Fail(
                        "Subject not found",
                        ResponseType.NotFound);
                }

                // 🔹 Soft Delete
                subject.IsActive = false;

                await _uow.SubjectRepository.Update(subject);
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success("Deletion Info",
                    "Subject deleted successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to delete subject",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<string>> UpdateSubject(UpdateSubjectRequest request)
        {
            try
            {
                // 🔹 Validate SubjectId
                if (request.SubjectId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid subject identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate Title
                if (string.IsNullOrWhiteSpace(request.Title))
                {
                    return ApiResponse<string>.Fail(
                        "Subject title is required",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate DepartmentId
                if (request.DepartmentId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid department identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate SemesterId
                if (request.SemesterId == Guid.Empty)
                {
                    return ApiResponse<string>.Fail(
                        "Invalid semester identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Get Existing Subject
                var subject = await _uow.SubjectRepository
                    .GetByIdAsync(request.SubjectId);

                if (subject == null)
                {
                    return ApiResponse<string>.Fail(
                        "Subject not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Department Exists
                var departmentExists = await _uow.DepartmentRepository.Query()
                    .AnyAsync(d => d.DepartmentId == request.DepartmentId);

                if (!departmentExists)
                {
                    return ApiResponse<string>.Fail(
                        "Department not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Semester Exists
                var semesterExists = await _uow.SemesterRepo.Query()
                    .AnyAsync(s => s.SemesterId == request.SemesterId);

                if (!semesterExists)
                {
                    return ApiResponse<string>.Fail(
                        "Semester not found",
                        ResponseType.NotFound);
                }

                // 🔹 Validate Faculty (optional)
                if (request.FacultyId.HasValue)
                {
                    var facultyExists = await _uow.FucaltyRepo.Query()
                        .AnyAsync(f => f.FacultyId == request.FacultyId.Value);

                    if (!facultyExists)
                    {
                        return ApiResponse<string>.Fail(
                            "Faculty not found",
                            ResponseType.NotFound);
                    }
                }

                // 🔹 Duplicate Check (excluding current subject)
                var normalizedTitle = request.Title.Trim().ToLower();

                var duplicateExists = await _uow.SubjectRepository.Query()
                    .AnyAsync(s =>
                        s.SubjectId != request.SubjectId &&
                        s.DepartmentId == request.DepartmentId &&
                        s.SemesterId == request.SemesterId &&
                        s.Title.Trim().ToLower() == normalizedTitle &&
                        s.IsActive);

                if (duplicateExists)
                {
                    return ApiResponse<string>.Fail(
                        "Subject already exists in this department and semester",
                        ResponseType.BadRequest);
                }

                // 🔹 Update Fields
                subject.Title = request.Title.Trim();
                subject.DepartmentId = request.DepartmentId;
                subject.SemesterId = request.SemesterId;
                subject.FacultyId = request.FacultyId;
                subject.CreditHours = request.CreditHours;
                subject.IsActive = request.IsActive;

                // 🔹 Save Changes
                await _uow.SubjectRepository.Update(subject);
                await _uow.SaveChangesAsync();

                return ApiResponse<string>.Success("Success Updation",
                    "Subject updated successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<string>.Fail(
                    "Failed to update subject",
                    ResponseType.InternalServerError);
            }
        }   
    }
}
