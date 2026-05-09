using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.AnnouncemenRepo;
using Domain_Service.RepoInterfaces.ClerkRepo;
using Domain_Service.RepoInterfaces.DeptRepo;
using Domain_Service.RepoInterfaces.FacultyRepo;
using Domain_Service.RepoInterfaces.NotificationRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UserManagment;

namespace Domain_Service.RepoInterfaces.UnitOfWork
{
    public interface IUnitOfWork 
    {
        IUserRepo UserRepo { get; }
        IUserRoleRepo UserRoleRepo { get; }
        IUserCreadentialRepo UserCreadentialRepo { get; }
        IStudentRepo StudentRepo { get; }
        ISessionRepo SessionRepo { get; }
        ISemesterRepo SemesterRepo { get; }
        IDepartmentRepository DepartmentRepository { get; }
        IAnnouncmentRepo AnnouncmentRepo { get; }
        INotificationRepo NotificationRepo { get; }
        IFacultyRepo FucaltyRepo { get; }
        IClerkRepo ClerkRepo { get; }

        /// <summary>
        /// Commits all changes to the database within an optional transaction
        /// </summary>
        Task<int> SaveChangesAsync();

        /// <summary>
        /// Executes multiple operations in a transaction
        /// </summary>
        Task ExecuteInTransactionAsync(Func<Task> operations, bool autosaveChanges = true);
    }
}
