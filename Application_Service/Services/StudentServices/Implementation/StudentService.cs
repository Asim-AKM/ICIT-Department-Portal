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
        public async Task<ApiResponse<List<GetStudentDto>>> GetStudentListBySessionIdAsync(GetStudentBySessionRequest getStudentBySessionRequest)
        {
            try
            {
                //  Input validation
                if (getStudentBySessionRequest.SessionId == Guid.Empty)
                    return ApiResponse<List<GetStudentDto>>.Fail("Invalid session identifier", ResponseType.BadRequest);

                //  Fetch data 
                var students = await _uow.StudentRepo.GetStudentListBySessionIdAsync(getStudentBySessionRequest.SessionId, getStudentBySessionRequest.StudentStatus);

                //  Handle empty results gracefully
                if (students == null || !students.Any())
                {
                    return ApiResponse<List<GetStudentDto>>.Success(
                        new List<GetStudentDto>(),
                        "No students found for the specified session and StudentStatus",
                        ResponseType.Ok);
                }

                //  Map to DTO
                var studentDtos = students.MapStudentListToGetStudentDto();



                //  Return standardized response
                return ApiResponse<List<GetStudentDto>>.Success(
                    studentDtos,
                    "Students retrieved successfully",
                    ResponseType.Ok);
            }
            catch (Exception)
            {


                return ApiResponse<List<GetStudentDto>>.Fail(
                    "An error occurred while retrieving students",
                    ResponseType.InternalServerError);
            }
        }
       
        public async Task<ApiResponse<string>> UploadStudentsFromExcelAsync(IFormFile file, Guid sessionId)
        {
            try
            {
                if (file == null || file.Length == 0)
                    return ApiResponse<string>.Fail("File", "Invalid file uploaded", ResponseType.BadRequest);

                // Get session
                var session = await _uow.SessionRepo.FirstOrDefaultAsync(s => s.SessionId == sessionId);
                if (session == null)
                    return ApiResponse<string>.Fail("Session", "Session Not Found", ResponseType.BadRequest);

                //  Get semesters
                var semesters = await _uow.SemesterRepo.GetSemesterBySessionIdAsync(sessionId);
                if (semesters == null || !semesters.Any())
                    return ApiResponse<string>.Fail("Semester", "No semesters found for this session", ResponseType.BadRequest);

                var firstSemester = semesters.FirstOrDefault(s => s.Order == 1);
                if (firstSemester == null)
                    throw new Exception("Semester 1 not found.");

                // Fetch all existing students for the session in a single query
                var existingStudents = await _uow.StudentRepo
                    .GetStudentListBySessionIdAsync(sessionId); // returns List<Student>

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
                            GPA = 0,
                            SamesterId = firstSemester.SemesterId,
                            SessionId = sessionId
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

        public async Task<ApiResponse<string>> VerifyStudentAsync(StudentVerifyRequest studentVerifyRequest)
        {
            var existStudent = await _uow.StudentRepo.GetByIdAsync(studentVerifyRequest.StudentId);
            if (existStudent == null)
            {
                return ApiResponse<string>.Fail("Student Not Found", ResponseType.NotFound);
            }
            if (existStudent.Status == StudentStatus.Verified)
            {
                return ApiResponse<string>.Fail("Student Already Verified", ResponseType.BadRequest);
            }

            // Generate TempPassword
            var tempPass = PasswordGenerator.Generate();

            // Create User
            var user = new User
            {
                UserId = existStudent.UserId,
                CreatedAt = DateTime.Now,
                Email = existStudent.StudentEmail,
                FullName = existStudent.StudentName,
                CNIC = existStudent.CNIC,
                Status = UserStatus.Active
            };
            try
            {
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    // Entry in UserEntity
                    await _uow.UserRepo.CreateAsync(user);

                    // assign credentials
                    var cread = user.MapToCreadDomain(tempPass);
                    await _uow.UserCreadentialRepo.CreateAsync(cread);

                    // Assign Role
                    var role = user.MapToUserRoleDomain(RoleType.Students);
                    await _uow.UserRoleRepo.CreateAsync(role);

                    //  Update Student Status
                    existStudent.Status = studentVerifyRequest.Status;
                    await _uow.StudentRepo.Update(existStudent);

                    //  Send Email 
                    var emailSent = await _emailService.SendStudentVerificationEmail(
                        existStudent.StudentEmail,
                        existStudent.StudentName,
                        existStudent.CNIC,
                        tempPass);
                    if (!emailSent)
                    {
                        // throw exception to rollback transaction
                        throw new Exception("No Internet Connection");
                    }
                });

                return ApiResponse<string>.Success("Student Verification", "Student has been Verified successfully", ResponseType.Created);
            }
            catch (Exception ex)
            {

                return ApiResponse<string>.Fail($"Failed to Verify this Student : {ex.Message}", ResponseType.BadRequest);
            }
        }

        public async Task<ApiResponse<BulkVerifyResultResponse>> VerifyStudentsBulkAsync(StudentBulkVerifyRequest bulkVerifyRequest)
        {
            var students = await _uow.StudentRepo.GetStudentsByIdsAsync(bulkVerifyRequest.StudentIds);

            if (students == null || !students.Any())
                return ApiResponse<BulkVerifyResultResponse>.Fail("No students found", ResponseType.NotFound);

            var result = new BulkVerifyResultResponse
            {
                Total = students.Count
            };

            // Separate students
            var alreadyVerified = students
                .Where(s => s.Status == StudentStatus.Verified)
                .ToList();

            var studentsToProcess = students
                .Where(s => s.Status == StudentStatus.Unverified)
                .ToList();

            result.AlreadyVerified.AddRange(alreadyVerified.Select(s => s.StudentName));

            if (!studentsToProcess.Any())
                return ApiResponse<BulkVerifyResultResponse>.Success(result, "All students already verified", ResponseType.Ok);

            var usersToAdd = new List<User>();
            var credsToAdd = new List<UserCredential>();
            var rolesToAdd = new List<UserRole>();
            var studentsToUpdate = new List<Student>();

            var emailQueue = new List<(string email, string name, string cnic, string password)>();

            try
            {
                await _uow.ExecuteInTransactionAsync(async () =>
                {
                    foreach (var student in studentsToProcess)
                    {
                        try
                        {
                            var tempPassword = PasswordGenerator.Generate();

                            var user = new User
                            {
                                UserId = student.UserId,
                                Email = student.StudentEmail,
                                FullName = student.StudentName,
                                CreatedAt = DateTime.UtcNow,
                                CNIC = student.CNIC,
                                Status = UserStatus.Active,
                            };

                            usersToAdd.Add(user);
                            credsToAdd.Add(user.MapToCreadDomain(tempPassword));
                            rolesToAdd.Add(user.MapToUserRoleDomain(RoleType.Students));

                            student.Status = bulkVerifyRequest.Status;
                            studentsToUpdate.Add(student);

                            emailQueue.Add((student.StudentEmail, student.StudentName, student.CNIC, tempPassword));

                            result.Success++;
                        }
                        catch
                        {
                            result.Failed++;
                            result.FailedStudents.Add(student.StudentName);
                        }
                    }

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

                // Send emails after DB commit
                foreach (var mail in emailQueue)
                {
                    await _emailService.SendStudentVerificationEmail(
                        mail.email,
                        mail.name,
                        mail.cnic,
                        mail.password);
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

    }
}
