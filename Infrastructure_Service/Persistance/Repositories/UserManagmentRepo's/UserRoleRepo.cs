using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRoleRepo : Repository<UserRole>,IUserRoleRepo
    {
        private readonly ApplicationDbContext _context;
        public UserRoleRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<RoleType>> GetUserRoleByUserId(Guid UserId)
        {
            return _context.UserRoles.Where(u => u.UserId == UserId).Select(ur => ur.RoleName).ToList();
        }
    }
}
