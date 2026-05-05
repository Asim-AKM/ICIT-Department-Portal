using Domain_Service.Entities.Academic;
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

        public async Task<List<Session>> GetSessionsByStatusAsync(SessionStatus? status = null)
        {
            var query = _context.Sessions.AsNoTracking();

            if (status.HasValue)
            {
                query = query.Where(x => x.Status == status.Value);
            }

            return await query
                .OrderByDescending(o => o.StartDate)
                .ToListAsync();
        }
    }
}
