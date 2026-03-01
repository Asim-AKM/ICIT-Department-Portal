using Domain_Service.RepoInterfaces.AdminRepo;
using Infrastructure_Service.Data;

namespace Infrastructure_Service.Persistance.Repositories.AdminRepo_s
{
    public class SessionRepo : ISessionRepo
    {
        private readonly ApplicationDbContext _context;
        public SessionRepo(ApplicationDbContext applicationDbContext)
        {
            _context = applicationDbContext;
        }
    }
}
