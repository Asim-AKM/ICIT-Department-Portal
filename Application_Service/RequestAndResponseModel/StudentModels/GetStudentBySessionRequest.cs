using Domain_Service.Enum;

namespace Application_Service.RequestAndResponseModel.StudentModels
{
    public class GetStudentBySessionRequest
    {
        public Guid SessionId { get; set; }
        public Guid DepartmentId { get; set; }
        public StudentStatus StudentStatus { get; set; }
    }
}
