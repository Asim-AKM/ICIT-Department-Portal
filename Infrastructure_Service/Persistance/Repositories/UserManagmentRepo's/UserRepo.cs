using Domain_Service.Entities.UserManagmentModule;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRepo(ApplicationDbContext context) : IUserRepo
    {
        private readonly ApplicationDbContext _context = context;
        public async Task<List<User>> GetAllUsersAsync()
        {
           return await _context.Users.ToListAsync();
        }
        public Task<User?> GetByIdentifier(string useridentifier)
        {
           return _context.Users.Where(u => u.UserName == useridentifier || u.Email == useridentifier || u.Contact == useridentifier).FirstOrDefaultAsync();
        }
    }
}
