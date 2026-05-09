using Domain_Service.Entities.Identity;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.UserManagment
{
    public interface IUserRepo : IRepository<User>
    {
        Task<User?> GetByIdentifier(string useridentifier);
        Task<List<Guid>> GetExistingUserIdsAsync(List<Guid> userIds);
        Task<bool> ExistsByEmailOrCNICAsync(string email, string cnic);

    }
}
