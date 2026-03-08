using Application_Service.Common;
using Application_Service.DTO_s.StudentDTO_s;
using Application_Service.Mapper_s.StudentManagmenMappers;
using Application_Service.Services.AdminServices.Interfaces;
using Application_Service.Services.StudentServices.Interfaces;
using ClosedXML.Excel;
using Domain_Service.Entities.StudentModule;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Microsoft.AspNetCore.Http;

namespace Application_Service.Services.StudentServices.Implementation
{
    public class StudentService : IStudentService
    {
        private readonly IUnitOfWork _uow;
        private readonly ISessionService _sessionService;
        public StudentService(IUnitOfWork unitOfWork, ISessionService sessionService)
        {
            _uow = unitOfWork;
            _sessionService = sessionService;
        }
        public async Task<ApiResponse<List<GetStudentDto>>> GetStudentListBySessionIdAsync(Guid sessionId)
        {
            try
            {
                //  Input validation
                if (sessionId == Guid.Empty)
                    return ApiResponse<List<GetStudentDto>>.Fail("Invalid session identifier", ResponseType.BadRequest);

                //  Fetch data 
                var students = await _uow.StudentRepo.GetStudentListBySessionIdAsync(sessionId);

                //  Handle empty results gracefully
                if (students == null || !students.Any())
                {
                    return ApiResponse<List<GetStudentDto>>.Success(
                        new List<GetStudentDto>(),
                        "No students found for the specified session",
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
            catch (Exception ex)
            {


                return ApiResponse<List<GetStudentDto>>.Fail(
                    "An error occurred while retrieving students",
                    ResponseType.InternalServerError);
            }
        }

        /// <summary>
        /// Uploads student records from an Excel file to the database for the specified academic session.
        /// </summary>
        /// <remarks>The method validates the uploaded file and ensures that the session and its semesters
        /// exist before processing. It checks for duplicate roll numbers and invalid data, and will fail if any are
        /// found. Exceptions are thrown for missing or invalid data, or duplicate roll numbers.</remarks>
        /// <param name="file">The Excel file containing student data to be uploaded. Must not be null or empty.</param>
        /// <param name="sessionId">The unique identifier of the session under which the students will be uploaded.</param>
        /// <returns>An ApiResponse containing a success message if the upload is successful; otherwise, an error message
        /// describing the reason for failure.</returns>
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
                            Status = StudentStatus.Unvarified,
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
    }
}
