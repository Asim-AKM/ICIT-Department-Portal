using Domain_Service.Entities.Shared;
using Domain_Service.RepoInterfaces.AnnouncemenRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;

namespace Infrastructure_Service.Persistance.Repositories.AnnouncementRepo_s
{
    public class AnnouncmentRepo : Repository<Announcement> , IAnnouncmentRepo
    {
        private readonly ApplicationDbContext _context;
        public AnnouncmentRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
