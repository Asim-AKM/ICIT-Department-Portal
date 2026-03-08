using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.AdminRepo
{
    public interface ISessionRepo : IRepository<Session>
    {
        Task<List<Session>> GetActiveSessionsAsync();
    }
}
