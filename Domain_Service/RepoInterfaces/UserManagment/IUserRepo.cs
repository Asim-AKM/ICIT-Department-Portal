using Domain_Service.Entities.Identity;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.UserManagment
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User?> GetByIdentifier(string useridentifier);
        
    }
}
