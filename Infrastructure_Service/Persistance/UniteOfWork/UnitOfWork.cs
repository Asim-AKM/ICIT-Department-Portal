using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.GenricRepo;
using Domain_Service.RepoInterfaces.UnitOfWork;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s;

namespace Infrastructure_Service.Persistance.UniteOfWork
{
    public class UnitOfWork : IUnitOfWork
    {
        private readonly ApplicationDbContext _dbContext;

        public UnitOfWork(ApplicationDbContext context)
        {
            _dbContext = context; 
        }

        // ==========================
        // Lazy initialization for Repositories
        // Property tabhi create hogi jab first time access hoga
        // ==========================

        private IRepository<User>? _users;
        public IRepository<User> Users
        {
            get
            {
                if (_users == null)
                {
                    _users = new Repository<User>(_dbContext); // Agar null hai, create repository
                }
                return _users; // Otherwise existing return karo
            }
        }

        private IRepository<UserCredential>? _userCreads;
        public IRepository<UserCredential> UserCreads => _userCreads ??= new Repository<UserCredential>(_dbContext); // Generic UserCredential repo

        private IRepository<UserRole>? _userRoles;
        public IRepository<UserRole> UserRoles => _userRoles ??= new Repository<UserRole>(_dbContext); // Generic UserRole repo

        private IUserRepo? _userRepo;
        public IUserRepo UserRepo => _userRepo ??= new UserRepo(_dbContext); // Custom repo for User

        private IUserRoleRepo? _userRoleRepo;
        public IUserRoleRepo UserRoleRepo => _userRoleRepo ??= new UserRoleRepo(_dbContext); // Custom repo for UserRole

        private IUserCreadentialRepo? _userCreadentialRepo;
        public IUserCreadentialRepo UserCreadentialRepo => _userCreadentialRepo ??= new UserCreadentialRepo(_dbContext); // Custom repo for UserCredential

        // ==========================
        // SaveChanges method
        // Changes ko database me commit karta hai
        // ==========================
        public async Task<int> SaveChangesAsync()
        {
            return await _dbContext.SaveChangesAsync();
        }
    }
}
