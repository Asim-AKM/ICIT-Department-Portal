using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.AdminRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;

namespace Infrastructure_Service.Persistance.Repositories.AdminRepo_s
{
    public class SessionRepo : Repository<Session>, ISessionRepo
    {
        private readonly ApplicationDbContext _context;
        public SessionRepo(ApplicationDbContext applicationDbContext) : base(applicationDbContext)
        {
            _context = applicationDbContext;
        }
    }
}
