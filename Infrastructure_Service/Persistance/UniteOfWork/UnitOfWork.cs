using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.AnnouncemenRepo;
using Domain_Service.RepoInterfaces.ClerkRepo;
using Domain_Service.RepoInterfaces.DeptRepo;
using Domain_Service.RepoInterfaces.EnrollmentRepo;
using Domain_Service.RepoInterfaces.FacultyRepo;
using Domain_Service.RepoInterfaces.GradeRepositories;
using Domain_Service.RepoInterfaces.NotificationRepo;
using Domain_Service.RepoInterfaces.ResultLockedRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.SubjectRepo_s;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.Repositories.AdminRepo_s;
using Infrastructure_Service.Persistance.Repositories.AnnouncementRepo_s;
using Infrastructure_Service.Persistance.Repositories.ClerkRepo_s;
using Infrastructure_Service.Persistance.Repositories.DeptRepo_s;
using Infrastructure_Service.Persistance.Repositories.EnrollmentRepo_s;
using Infrastructure_Service.Persistance.Repositories.FacultyRepo_s;
using Infrastructure_Service.Persistance.Repositories.GradeRepo_s;
using Infrastructure_Service.Persistance.Repositories.NotificationRepo_s;
using Infrastructure_Service.Persistance.Repositories.ResultLockRepo_s;
using Infrastructure_Service.Persistance.Repositories.StudentRepo_s;
using Infrastructure_Service.Persistance.Repositories.SubjectRepositories;
using Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s;

namespace Infrastructure_Service.Persistance.UniteOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;
        // Repositories
        public IUserRepo UserRepo { get; }
        public IUserRoleRepo UserRoleRepo { get; }
        public IUserCreadentialRepo UserCreadentialRepo { get; }
        public IStudentRepo StudentRepo { get; }
        public ISessionRepo SessionRepo { get; }
        public ISemesterRepo SemesterRepo { get; }
        public IDepartmentRepository DepartmentRepository { get; }
        public IAnnouncmentRepo  AnnouncmentRepo { get; }
        public INotificationRepo NotificationRepo { get; }
        public IClerkRepo ClerkRepo { get; }
        public IFacultyRepo FucaltyRepo { get; }
        public ISubjectRepository SubjectRepository { get; }
        public IEnrollmentRepo EnrollmentRepo { get; }
        public IGradeRepo GradeRepo { get; }
        public IResultLockRepo ResultLockRepo { get; }


        //  FIXED: All repositories will share the SAME DbContext
        public UnitOfWork(ApplicationDbContext dbContext)
        {
            _dbContext = dbContext ?? throw new ArgumentNullException(nameof(dbContext));

            //  Pass the SAME dbContext to all repositories
            UserRepo = new UserRepo(_dbContext);
            UserRoleRepo = new UserRoleRepo(_dbContext);
            UserCreadentialRepo = new UserCreadentialRepo(_dbContext);
            StudentRepo = new StudentRepo(_dbContext);
            SessionRepo = new SessionRepo(_dbContext);
            SemesterRepo = new SemeterRepo(_dbContext);
            DepartmentRepository = new DepartmentRepository(_dbContext);
            AnnouncmentRepo = new AnnouncmentRepo(_dbContext);
            NotificationRepo = new NotificationRepo(_dbContext);
            FucaltyRepo = new FacultyRepo(_dbContext);
            ClerkRepo = new ClerkRepo(_dbContext);
            SubjectRepository = new SubjectRepository(_dbContext);
            EnrollmentRepo = new EnrollmentRepo(_dbContext);
            GradeRepo = new GradeRepo(_dbContext);
            ResultLockRepo = new ResultLockRepo(_dbContext);

        }

        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        public async Task ExecuteInTransactionAsync(Func<Task> operations, bool autosaveChanges = true)
        {
            if (operations == null)
                throw new ArgumentNullException(nameof(operations));

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();

            try
            {
                await operations();
                if (autosaveChanges)
                {
                    await SaveChangesAsync();
                }
                await transaction.CommitAsync();
            }
            catch
            {
                await transaction.RollbackAsync();
                throw;
            }
        }
    }
}