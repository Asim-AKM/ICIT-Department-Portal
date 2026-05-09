using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Entities.Academic;
using Domain_Service.Enum;

namespace Application_Service.Mapper_s.StudentManagmenMappers
{
    public static class StudentManagmentMappers
    {
        public static Session MapToSession(this SessionAddDto dto)
        {
            return new Session
            {
              SessionId = new Guid(),
                Name = dto.Name,
                StartDate = dto.StartDate,
                EndDate = dto.EndDate,
                Status = SessionStatus.Active
            };
        }

        public static List<GetStudentDto> MapStudentListToGetStudentDto(this List<Student> students,Dictionary<Guid, string> semesterDict)
        {
            if (students == null || students.Count == 0)
                return new List<GetStudentDto>();

            return students.Select(student => new GetStudentDto(
                student.StudentId,
                student.UserId,
                student.RegistrationNo,
                student.RollNo,
                student.SamesterId,
                semesterDict.ContainsKey(student.SamesterId)
                    ? semesterDict[student.SamesterId]
                    : string.Empty,
                student.SessionId,
                student.StudentName,
                student.StudentEmail,
                student.CNIC,
                student.DepartmentId,
                student.Status.ToString()
            )).ToList();
        }
    }
}
