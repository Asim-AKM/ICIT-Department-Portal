using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRoleRepo : IUserRoleRepo
    {
        private readonly ApplicationDbContext _context;
        public UserRoleRepo(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
