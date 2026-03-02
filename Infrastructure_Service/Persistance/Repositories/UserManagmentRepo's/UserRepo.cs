using Domain_Service.Entities.UserManagmentModule;
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

        public Task<User?> GetByIdentifier(string useridentifier)
        {
           return _context.Users.Where(u => u.UserName == useridentifier || u.Email == useridentifier || u.Contact == useridentifier).FirstOrDefaultAsync();
        }
    }
}
