using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.UserManagment;

namespace Domain_Service.RepoInterfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<UserCredential> UserCreads { get; }
        IRepository<UserRole> UserRoles { get; }
        IUserRepo UserRepo { get; }
        IUserRoleRepo UserRoleRepo { get; }
        IUserCreadentialRepo UserCreadentialRepo { get; }

        Task<int> SaveChangesAsync();

    }
}
