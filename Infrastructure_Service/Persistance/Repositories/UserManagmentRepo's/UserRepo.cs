using Domain_Service.Entities.Identity;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRepo :Repository<User>, IUserRepo 
    {
        private readonly ApplicationDbContext _context;
        public UserRepo(ApplicationDbContext context): base(context)
        {
            _context = context;
        }

        public async  Task<User?> GetByIdentifier(string useridentifier)
        {
           return await _context.Users.Where(u => u.CNIC == useridentifier).FirstOrDefaultAsync();
        }
    }
}
