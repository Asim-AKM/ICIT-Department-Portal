using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.AdminRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.AdminRepo_s
{
    public class SemeterRepo : Repository<Semester>, ISemesterRepo
    {
        private readonly ApplicationDbContext _context;
        public SemeterRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public Task<List<Semester>> GetSemesterBySessionIdAsync(Guid sessionId)
        {
           return _context.Semesters.AsNoTracking().Where(x=> x.SessionId == sessionId).ToListAsync();
        }
    }
}
