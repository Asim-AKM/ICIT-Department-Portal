using Domain_Service.Entities.Identity;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserRepo : Repository<User>, IUserRepo
    {
       
        private readonly ApplicationDbContext _context;

        public UserRepo(ApplicationDbContext context) : base(context)
        {
            
            _context = context;
        }

        public async Task<User?> GetByIdentifier(string useridentifier)
        {
            return await _context.Users
                .Where(u => u.CNIC == useridentifier)
                .FirstOrDefaultAsync();
        }
        public async Task<List<Guid>> GetExistingUserIdsAsync(List<Guid> userIds)
        {
            return await _context.Users
                .Where(x => userIds.Contains(x.UserId))
                .Select(x => x.UserId)
                .ToListAsync();
        }

        public async Task<bool> ExistsByEmailOrCNICAsync(string email, string cnic)
        {
            return await _context.Users
                .AsNoTracking()
                .AnyAsync(u =>
                    u.Email == email ||
                    u.CNIC == cnic);
        }
    }
}

