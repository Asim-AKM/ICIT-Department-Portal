using Domain_Service.Entities.Identity;
using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserCreadentialRepo : Repository<UserCredential>,IUserCreadentialRepo
    {
        private readonly ApplicationDbContext _context;
        public UserCreadentialRepo(ApplicationDbContext applicationDb) : base(applicationDb)
        {
            _context = applicationDb;
        }
    }
}
