using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.AdminRepo
{
    public interface ISemesterRepo : IRepository<Semester>
    {
        Task<List<Semester>> GetSemesterBySessionIdAsync(Guid sessionId);
    }
}
