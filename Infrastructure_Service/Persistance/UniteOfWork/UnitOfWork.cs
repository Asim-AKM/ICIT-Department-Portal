using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.DeptRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.Repositories.DeptRepo_s;

namespace Infrastructure_Service.Persistance.UniteOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        // Repositories injected
        public IUserRepo UserRepo { get; }
        public IUserRoleRepo UserRoleRepo { get; }
        public IUserCreadentialRepo UserCreadentialRepo { get; }
        public IStudentRepo StudentRepo { get; }
        public ISessionRepo SessionRepo { get; }
        public ISemesterRepo SemesterRepo { get; }
        public IDepartmentRepository DepartmentRepository { get; }

        // Constructor: all dependencies injected
        public UnitOfWork(
            ApplicationDbContext dbContext, IUserRepo userRepo,
            IUserRoleRepo userRoleRepo,
            IUserCreadentialRepo userCreadentialRepo,
            IStudentRepo studentRepo,
            ISessionRepo sessionRepo,
            ISemesterRepo semesterRepo,
            IDepartmentRepository departmentRepository
         )
        {
            _dbContext = dbContext;
            UserRepo = userRepo;
            UserRoleRepo = userRoleRepo;
            UserCreadentialRepo = userCreadentialRepo;
            StudentRepo = studentRepo;
            SessionRepo = sessionRepo;
            SemesterRepo = semesterRepo;
            DepartmentRepository = departmentRepository;

        }

        /// <summary>
        /// Save changes to DB
        /// </summary>
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }

        /// <summary>
        /// Execute multiple repository operations in a transaction
        /// </summary>
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

        /// <summary>
        /// Properly dispose DbContext
        /// </summary>
        public void Dispose()
        {
            _dbContext?.Dispose();
        }
    }
}
