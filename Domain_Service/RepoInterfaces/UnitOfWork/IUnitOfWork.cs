using Domain_Service.Entities.StudentModule;
using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UserManagment;

namespace Domain_Service.RepoInterfaces.UnitOfWork
{
    public interface IUnitOfWork
    {
        IRepository<User> Users { get; }
        IRepository<UserCredential> UserCreads { get; }
        IRepository<UserRole> UserRoles { get; }
        IRepository<Session> Sessions { get; }
        IRepository<Student> Students { get; }
        IUserRepo UserRepo { get; }
        IUserRoleRepo UserRoleRepo { get; }
        IUserCreadentialRepo UserCreadentialRepo { get; }

        IStudentRepo StudentRepo { get; }
        ISessionRepo SessionRepo { get; }

        Task<int> SaveChangesAsync();

    }
}
