using Domain_Service.Entities.StudentModule;
using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UserManagment;

namespace Domain_Service.RepoInterfaces.UnitOfWork
{
    public interface IUnitOfWork : IDisposable
    {
        IUserRepo UserRepo { get; }
        IUserRoleRepo UserRoleRepo { get; }
        IUserCreadentialRepo UserCreadentialRepo { get; }
        IStudentRepo StudentRepo { get; }
        ISessionRepo SessionRepo { get; }

        /// <summary>
        /// Commits all changes to the database within an optional transaction
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Executes multiple operations in a transaction
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> operations);
    }
}
