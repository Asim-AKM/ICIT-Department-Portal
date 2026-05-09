namespace Application_Service.DTO_s.StudentDTO_s
{
    public record GetStudentDto(Guid StudentId, Guid UserId, string RegistrationNo, string RollNo, Guid SemesterId, string SemesterName, Guid SessionId,string StudentName, string StudentEmail,string CNIC,Guid Department, string Status);
    
}
