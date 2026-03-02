using Domain_Service.Entities.StudentModule;
using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.AdminRepo;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.StudentManagments;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Infrastructure_Service.Persistance.Repositories.AdminRepo_s;
using Infrastructure_Service.Persistance.Repositories.StudentRepo_s;
using Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s;

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

        // Constructor: all dependencies injected
        public UnitOfWork( 
            ApplicationDbContext dbContext, IUserRepo userRepo,
            IUserRoleRepo userRoleRepo,
            IUserCreadentialRepo userCreadentialRepo,
            IStudentRepo studentRepo,
            ISessionRepo sessionRepo
         )
        {
            _dbContext = dbContext;
            UserRepo = userRepo;
            UserRoleRepo = userRoleRepo;
            UserCreadentialRepo = userCreadentialRepo;
            StudentRepo = studentRepo;
            SessionRepo = sessionRepo;
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
        public async Task ExecuteInTransactionAsync(Func<Task> operations)
        {
            if (operations == null) throw new ArgumentNullException(nameof(operations));

            await using var transaction = await _dbContext.Database.BeginTransactionAsync();
            try
            {
                await operations();
                await SaveChangesAsync();
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
