using Domain_Service.RepoInterfaces.UserManagment;
using Infrastructure_Service.Data;

namespace Infrastructure_Service.Persistance.Repositories.UserManagmentRepo_s
{
    public class UserCreadentialRepo : IUserCreadentialRepo
    {
        private readonly ApplicationDbContext _context;
        public UserCreadentialRepo(ApplicationDbContext applicationDb)
        {
            _context = applicationDb;
        }
    }
}
