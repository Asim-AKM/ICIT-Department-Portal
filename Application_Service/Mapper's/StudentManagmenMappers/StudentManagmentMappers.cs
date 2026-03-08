using Application_Service.DTO_s.StudentDTO_s;
using Domain_Service.Entities.StudentModule;
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

        public static List<GetStudentDto> MapStudentListToGetStudentDto(this List<Student> students)
        {
            if (students != null && students.Count > 0)
            {
                List<GetStudentDto> studentDtoList = new List<GetStudentDto>();

                foreach (var student in students)
                {
                    GetStudentDto dto = new GetStudentDto
                        (
                        student.StudentId, student.UserId, student.RegistrationNo, student.RollNo,
                        student.SamesterId, student.SessionId, student.StudentName, student.StudentEmail,
                        student.CNIC, student.Status.ToString()
                        );
                    studentDtoList.Add(dto);
                }

                return studentDtoList;
            }

            return new List<GetStudentDto>(); 
        }
    }
}
