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

        private IRepository<Student>? _students;
        public IRepository<Student> Students => _students ??= new Repository<Student>(_dbContext); // Generic Student repo

        private IRepository<Session> _sessions;
        public IRepository<Session> Sessions => _sessions ??= new Repository<Session>(_dbContext); // Generic Session repo

        private IUserRepo? _userRepo;
        public IUserRepo UserRepo => _userRepo ??= new UserRepo(_dbContext); // Custom repo for User

        private IUserRoleRepo? _userRoleRepo;
        public IUserRoleRepo UserRoleRepo => _userRoleRepo ??= new UserRoleRepo(_dbContext); // Custom repo for UserRole

        private IUserCreadentialRepo? _userCreadentialRepo;
        public IUserCreadentialRepo UserCreadentialRepo => _userCreadentialRepo ??= new UserCreadentialRepo(_dbContext); // Custom repo for UserCredential

        private IStudentRepo _studentRepo;
        public IStudentRepo StudentRepo => _studentRepo ??= new StudentRepo(_dbContext); // Custom repo for Student

        private ISessionRepo _sessionRepo;
        public ISessionRepo SessionRepo => _sessionRepo ??= new SessionRepo(_dbContext); // Custom repo for Session



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
