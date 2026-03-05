using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.StudentManagments
{
    public interface IStudentRepo : IRepository<Student>
    {
        Task<List<string>> StudentRollNoList(Guid sessionId);
        Task<List<Student>> GetStudentListBySessionIdAsync(Guid SessionId);
    }
}
