using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRoleRepo : Repository<UserRole>,IUserRoleRepo
    {
        private readonly ApplicationDbContext _context;
        public UserRoleRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
