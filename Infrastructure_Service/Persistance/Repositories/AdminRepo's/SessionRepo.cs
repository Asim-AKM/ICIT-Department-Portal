using Domain_Service.Entities.StudentModule;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.AdminRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.AdminRepo_s
{
    public class SessionRepo : Repository<Session>, ISessionRepo
    {
        private readonly ApplicationDbContext _context;
        public SessionRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }

        public async Task<List<Session>> GetActiveSessionsAsync()
        {
          return await  _context.Sessions.AsNoTracking()
                .Where(x=> x.Status == SessionStatus.Active)
                .OrderByDescending(o=> o.StartDate)
                .ToListAsync();
        }
    }
}
