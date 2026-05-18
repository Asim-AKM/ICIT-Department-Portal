using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.StudentManagmenMappers;
using Application_Service.Mapper_s.UserManagmentMappers;
using Application_Service.RequestAndResponseModel.StudentModels;
using Application_Service.Services.StudentServices.Interfaces;
using ClosedXML.Excel;
using Domain_Service.Entities.Academic;
using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.EmailRepo;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.AspNetCore.Http;
using Microsoft.EntityFrameworkCore;

namespace Application_Service.Services.StudentServices.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly IEmailRepository _emailService;
        public StudentService(IUnitOfWork unitOfWork, IEmailRepository emailRepository)
        {
            _uow = unitOfWork;
            _emailService = emailRepository;
        }
        public async Task<ApiResponse<List<GetStudentDto>>> GetStudentListBySessionIdAndDeprtIdAsync(GetStudentBySessionRequest request)
        {
            try
            {
                // 🔹 Validate SessionId
                if (request.SessionId == Guid.Empty)
                {
                    return ApiResponse<List<GetStudentDto>>.Fail(
                        "Invalid session identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate DepartmentId
                if (request.DepartmentId == Guid.Empty)
                {
                    return ApiResponse<List<GetStudentDto>>.Fail(
                        "Invalid department identifier",
                        ResponseType.BadRequest);
                }

                // 🔹 Validate Department Exists
                var department = await _uow.DepartmentRepository
                    .GetByIdAsync(request.DepartmentId);

                if (department == null)
                {
                    return ApiResponse<List<GetStudentDto>>.Fail(
                        "Department not found",
                        ResponseType.NotFound);
                }

                // 🔹 Fetch Students
                var students = await _uow.StudentRepo
                    .Query()
                    .Where(s =>
                        s.SessionId == request.SessionId &&
                        s.DepartmentId == request.DepartmentId &&
                        s.Status == request.StudentStatus
                    )
                    .ToListAsync();

                // 🔹 Empty Result
                if (!students.Any())
                {
                    return ApiResponse<List<GetStudentDto>>.Success(
                        new List<GetStudentDto>(),
                        "No students found",
                        ResponseType.Ok);
                }

                // 🔹 Fetch Semesters
                var semesters = await _uow.SemesterRepo.GetAllAsync();

                // 🔥 Fast Lookup Dictionary
                var semesterDict = semesters.ToDictionary(
                    s => s.SemesterId,
                    s => s.Name
                );

                // 🔹 Mapping
                var studentDtos = students.MapStudentListToGetStudentDto(semesterDict);

                return ApiResponse<List<GetStudentDto>>.Success(
                    studentDtos,
                    "Students retrieved successfully",
                    ResponseType.Ok);
            }
            catch
            {
                return ApiResponse<List<GetStudentDto>>.Fail(
                    "An error occurred while retrieving students",
                    ResponseType.InternalServerError);
            }
        }

        public async Task<ApiResponse<string>> UploadStudentsFromExcelAsync(UploadBulkStudentDto request, IFormFile file)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return ApiResponse<string>.Fail("File", "Invalid file uploaded", ResponseType.BadRequest);

                // Get session
                var session = await _uow.SessionRepo.FirstOrDefaultAsync(s => s.SessionId == request.sessionId);
                if (session == null)
                    return ApiResponse<string>.Fail("Session", "Session Not Found", ResponseType.BadRequest);

                //  Get semesters
                var semesters = await _uow.SemesterRepo.GetSemesterBySessionIdAsync(request.sessionId);
                if (semesters == null || !semesters.Any())
                    return ApiResponse<string>.Fail("Semester", "No semesters found for this session", ResponseType.BadRequest);

                var firstSemester = semesters.FirstOrDefault(s => s.Order == 1);
                if (firstSemester == null)
                    throw new Exception("Semester 1 not found.");

                // Fetch all existing students for the session in a single query
                var existingStudents = await _uow.StudentRepo
                    .GetStudentListBySessionIdAsync(request.sessionId); // returns List<Student>

                var existingRollNos = existingStudents.Select(s => s.RollNo).ToHashSet();
                var existingCnics = existingStudents.Select(s => s.CNIC).ToHashSet();
                var existingEmails = existingStudents.Select(s => s.StudentEmail).ToHashSet();

                // Transection Start
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    //  Read Excel with ClosedXML
                    using var stream = new MemoryStream();
                    await file.CopyToAsync(stream);
                    stream.Position = 0;

                    using var workbook = new XLWorkbook(stream);
                    var worksheet = workbook.Worksheet(1); // first worksheet

                    var lastRow = worksheet.LastRowUsed().RowNumber();

                    var studentsToInsert = new List<Student>();

                    for (int row = 2; row <= lastRow; row++) // skip header
                    {
                        var name = worksheet.Cell(row, 1).GetString().Trim();
                        var email = worksheet.Cell(row, 2).GetString().Trim();
                        var rollNo = worksheet.Cell(row, 3).GetString().Trim();
                        var regNo = worksheet.Cell(row, 4).GetString().Trim();
                        var cnic = worksheet.Cell(row, 5).GetString().Trim();
                        if (string.IsNullOrWhiteSpace(name) ||
                            string.IsNullOrWhiteSpace(email) ||
                            string.IsNullOrWhiteSpace(rollNo) ||
                            string.IsNullOrWhiteSpace(regNo) ||
                            string.IsNullOrWhiteSpace(cnic))
                        {
                            throw new Exception($"Invalid data at row {row}");
                        }

                        // Check duplicates and return immediately
                        if (existingRollNos.Contains(rollNo))
                            throw new Exception($"Duplicate RollNo at row {row}");
                        if (existingCnics.Contains(cnic))
                            throw new Exception($"Duplicate CNIC at row {row}");

                        if (existingEmails.Contains(email))
                            throw new Exception($"Duplicate Email at row {row}");

                        existingRollNos.Add(rollNo);
                        existingCnics.Add(cnic);
                        existingEmails.Add(email);


                        var student = new Student
                        {
                            StudentId = Guid.NewGuid(),
                            UserId = Guid.NewGuid(),
                            StudentName = name,
                            StudentEmail = email,
                            RollNo = rollNo,
                            RegistrationNo = regNo,
                            CNIC = cnic,
                            Status = StudentStatus.Unverified,
                            SamesterId = firstSemester.SemesterId,
                            SessionId = request.sessionId,
                            DepartmentId = request.DepartmentId
                        };

                        studentsToInsert.Add(student);
                    }
                    //  Insert all students
                    await _uow.StudentRepo.AddRangeAsync(studentsToInsert);
                });

                return ApiResponse<string>.Success("Students uploaded successfully", "Upload successful", ResponseType.Created);
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail($"Bulk Student Data Not Uploaded: {ex.Message}", ResponseType.BadRequest);
            }
        }

        public async Task<ApiResponse<string>> VerifyStudentAsync(StudentVerifyRequest request)
        {
            try
            {
                var student = await _uow.StudentRepo
                    .GetByIdAsync(request.StudentId);

                if (student == null)
                {
                    return ApiResponse<string>.Fail(
                        "Student Not Found",
                        ResponseType.NotFound);
                }

                // Already in same status
                if (student.Status == request.Status)
                {
                    return ApiResponse<string>.Fail(
                        "Student already in requested status",
                        ResponseType.BadRequest);
                }

                var existingUser = await _uow.UserRepo
                    .ExistsByEmailOrCNICAsync(student.StudentEmail, student.CNIC);

                var tempPassword = PasswordGenerator.Generate();
                var createUser = existingUser == null;

                try
                {
                    await _uow.ExecuteInTransactionAsync(async () =>
                    {
                        // 1. ALWAYS update student status
                        student.Status = request.Status;
                        await _uow.StudentRepo.Update(student);

                        // 2. Create user ONLY if not exists
                        if (createUser)
                        {
                            var user = new User
                            {
                                UserId = student.UserId,
                                CreatedAt = DateTime.UtcNow,
                                Email = student.StudentEmail,
                                FullName = student.StudentName,
                                CNIC = student.CNIC,
                                Status = UserStatus.Active,
                                DepartmentId = student.DepartmentId
                            };

                            await _uow.UserRepo.CreateAsync(user);

                            await _uow.UserCreadentialRepo.CreateAsync(
                                user.MapToCreadDomain(tempPassword));

                            await _uow.UserRoleRepo.CreateAsync(
                                user.MapToUserRoleDomain(RoleType.Student));
                        }

                    }, autosaveChanges: false);

                    await _uow.SaveChangesAsync();

                    // 3. Email AFTER commit (safe)
                    if (createUser)
                    {
                        await _emailService.SendStudentVerificationEmail(
                            student.StudentEmail,
                            student.StudentName,
                            student.CNIC,
                            tempPassword);
                    }

                    return ApiResponse<string>.Success(
                        "Student Verification",
                        "Student has been verified successfully",
                        ResponseType.Created);
                }
                catch (Exception ex)
                {
                    return ApiResponse<string>.Fail(
                        $"Failed to verify student: {ex.Message}",
                        ResponseType.BadRequest);
                }
            }
            catch (Exception ex)
            {
                return ApiResponse<string>.Fail(
                    $"An error occurred: {ex.Message}",
                    ResponseType.InternalServerError);
            }
        }

        #region BulkVerification Without Auto Enrollment
        //public async Task<ApiResponse<BulkVerifyResultResponse>> VerifyStudentsBulkAsync(StudentBulkVerifyRequest bulkVerifyRequest)
        //{
        //    try
        //    {
        //        var students = await _uow.StudentRepo
        //            .GetStudentsByIdsAsync(bulkVerifyRequest.StudentIds);

        //        if (students == null || !students.Any())
        //        {
        //            return ApiResponse<BulkVerifyResultResponse>.Fail(
        //                "No students found",
        //                ResponseType.NotFound);
        //        }

        //        var result = new BulkVerifyResultResponse
        //        {
        //            Total = students.Count
        //        };

        //        var usersToAdd = new List<User>();
        //        var credsToAdd = new List<UserCredential>();
        //        var rolesToAdd = new List<UserRole>();
        //        var studentsToUpdate = new List<Student>();

        //        var emailQueue = new List<(string email, string name, string cnic, string password)>();

        //        // Get existing users in ONE query
        //        var existingUserIds = await _uow.UserRepo.GetExistingUserIdsAsync(
        //            students.Select(x => x.UserId).ToList());

        //        await _uow.ExecuteInTransactionAsync(async () =>
        //        {
        //            foreach (var student in students)
        //            {
        //                try
        //                {
        //                    // Always update student status
        //                    student.Status = bulkVerifyRequest.Status;
        //                    studentsToUpdate.Add(student);

        //                    // If user already exists
        //                    // only update student status
        //                    if (existingUserIds.Contains(student.UserId))
        //                    {
        //                        result.Success++;
        //                        continue;
        //                    }

        //                    // Create new user only if not exists
        //                    var tempPassword = PasswordGenerator.Generate();

        //                    var user = new User
        //                    {
        //                        UserId = student.UserId,
        //                        Email = student.StudentEmail,
        //                        FullName = student.StudentName,
        //                        CNIC = student.CNIC,
        //                        DepartmentId = student.DepartmentId,
        //                        Status = UserStatus.Active,
        //                        CreatedAt = DateTime.UtcNow
        //                    };

        //                    usersToAdd.Add(user);

        //                    credsToAdd.Add(
        //                        user.MapToCreadDomain(tempPassword));

        //                    rolesToAdd.Add(
        //                        user.MapToUserRoleDomain(RoleType.Student));

        //                    emailQueue.Add((
        //                        student.StudentEmail,
        //                        student.StudentName,
        //                        student.CNIC,
        //                        tempPassword));

        //                    result.Success++;
        //                }
        //                catch
        //                {
        //                    result.Failed++;
        //                    result.FailedStudents.Add(student.StudentName);
        //                }
        //            }

        //            // Bulk Insert
        //            if (usersToAdd.Any())
        //                await _uow.UserRepo.AddRangeAsync(usersToAdd);

        //            if (credsToAdd.Any())
        //                await _uow.UserCreadentialRepo.AddRangeAsync(credsToAdd);

        //            if (rolesToAdd.Any())
        //                await _uow.UserRoleRepo.AddRangeAsync(rolesToAdd);

        //            // Bulk Update Students
        //            if (studentsToUpdate.Any())
        //                await _uow.StudentRepo.UpdatedRangeAsync(studentsToUpdate);

        //        }, autosaveChanges: false);

        //        await _uow.SaveChangesAsync();

        //        // Send emails after successful commit
        //        if (emailQueue.Any())
        //        {
        //            await Task.WhenAll(
        //                emailQueue.Select(mail =>
        //                    _emailService.SendStudentVerificationEmail(
        //                        mail.email,
        //                        mail.name,
        //                        mail.cnic,
        //                        mail.password)));
        //        }

        //        return ApiResponse<BulkVerifyResultResponse>.Success(
        //            result,
        //            "Student bulk verification completed successfully",
        //            ResponseType.Ok);
        //    }
        //    catch (Exception ex)
        //    {
        //        return ApiResponse<BulkVerifyResultResponse>.Fail(
        //            $"Bulk verification failed: {ex.Message}",
        //            ResponseType.BadRequest);
        //    }
        //}
        #endregion

        public async Task<ApiResponse<BulkVerifyResultResponse>> VerifyStudentsBulkAsync(StudentBulkVerifyRequest bulkVerifyRequest)
        {
            try
            {
                var students = await _uow.StudentRepo
                    .GetStudentsByIdsAsync(bulkVerifyRequest.StudentIds);

                if (students == null || !students.Any())
                {
                    return ApiResponse<BulkVerifyResultResponse>.Fail(
                        "No students found",
                        ResponseType.NotFound);
                }

                var result = new BulkVerifyResultResponse
                {
                    Total = students.Count
                };

                var usersToAdd = new List<User>();
                var credsToAdd = new List<UserCredential>();
                var rolesToAdd = new List<UserRole>();
                var studentsToUpdate = new List<Student>();

                var emailQueue = new List<(string email, string name, string cnic, string password)>();

                var existingUserIds = await _uow.UserRepo.GetExistingUserIdsAsync(
                    students.Select(x => x.UserId).ToList());

                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    foreach (var student in students)
                    {
                        try
                        {
                            // 🔹 Update Student Status
                            student.Status = bulkVerifyRequest.Status;
                            studentsToUpdate.Add(student);

                            // 🔹 Create User if not exists
                            if (!existingUserIds.Contains(student.UserId))
                            {
                                var tempPassword = PasswordGenerator.Generate();

                                var user = new User
                                {
                                    UserId = student.UserId,
                                    Email = student.StudentEmail,
                                    FullName = student.StudentName,
                                    CNIC = student.CNIC,
                                    DepartmentId = student.DepartmentId,
                                    Status = UserStatus.Active,
                                    CreatedAt = DateTime.UtcNow
                                };

                                usersToAdd.Add(user);
                                credsToAdd.Add(user.MapToCreadDomain(tempPassword));
                                rolesToAdd.Add(user.MapToUserRoleDomain(RoleType.Student));

                                emailQueue.Add((
                                    student.StudentEmail,
                                    student.StudentName,
                                    student.CNIC,
                                    tempPassword));
                            }

                            // 🔥 AUTO ENROLLMENT (ONLY WHEN VERIFIED)
                            if (bulkVerifyRequest.Status == StudentStatus.Verified)
                            {
                                bool exists = await EnrollmentExists(student.StudentId, student.SamesterId);

                                if (exists)
                                {
                                    result.skiped++;
                                    continue;
                                }

                                var subjects = await _uow.SubjectRepository.Query()
                                    .Where(s =>
                                        s.DepartmentId == student.DepartmentId &&
                                        s.SemesterId == student.SamesterId)
                                    .ToListAsync();

                                var enrollments = subjects.Select(subject => new Enrollment
                                {
                                    EnrollmentId = Guid.NewGuid(),
                                    StudentId = student.StudentId,
                                    SubjectId = subject.SubjectId,
                                    SemesterId = student.SamesterId,
                                    Status = EnrollmentStatus.Enrolled
                                }).ToList();

                                if (enrollments.Any())
                                {
                                    await _uow.EnrollmentRepo.AddRangeAsync(enrollments);
                                }
                            }

                            result.Success++;
                        }
                        catch
                        {
                            result.Failed++;
                            result.FailedStudents.Add(student.StudentName);
                        }
                    }

                    // 🔹 Bulk DB operations
                    if (usersToAdd.Any())
                        await _uow.UserRepo.AddRangeAsync(usersToAdd);

                    if (credsToAdd.Any())
                        await _uow.UserCreadentialRepo.AddRangeAsync(credsToAdd);

                    if (rolesToAdd.Any())
                        await _uow.UserRoleRepo.AddRangeAsync(rolesToAdd);

                    if (studentsToUpdate.Any())
                        await _uow.StudentRepo.UpdatedRangeAsync(studentsToUpdate);

                }, autosaveChanges: false);

                await _uow.SaveChangesAsync();


                if (emailQueue.Any())
                {
                    await Task.WhenAll(
                        emailQueue.Select(mail =>
                            _emailService.SendStudentVerificationEmail(
                                mail.email,
                                mail.name,
                                mail.cnic,
                                mail.password)));
                }

                return ApiResponse<BulkVerifyResultResponse>.Success(
                    result,
                    "Student bulk verification completed successfully",
                    ResponseType.Ok);
            }
            catch (Exception ex)
            {
                return ApiResponse<BulkVerifyResultResponse>.Fail(
                    $"Bulk verification failed: {ex.Message}",
                    ResponseType.BadRequest);
            }
        }
        private async Task<bool> EnrollmentExists(Guid studentId, Guid semesterId)
        {
            return await _uow.EnrollmentRepo.Query()
                .AnyAsync(e =>
                    e.StudentId == studentId &&
                    e.SemesterId == semesterId);
        }

    }
}
