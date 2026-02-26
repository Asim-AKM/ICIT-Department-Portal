using Domain_Service.Entities.UserManagmentModule;

namespace Domain_Service.RepoInterfaces.UserManagment
{
    public interface IUserRepo
    {
       Task<List<User>> GetAllUsersAsync();
        Task<User?> GetByIdentifier(string useridentifier);
    }
}
