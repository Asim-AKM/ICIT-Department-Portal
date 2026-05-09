using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.ClerkRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.ClerkRepo_s
{
    public class ClerkRepo : Repository<Clerk>, IClerkRepo
    {
        private readonly ApplicationDbContext _context;
        public ClerkRepo(ApplicationDbContext context ) : base( context ) 
        {
            _context = context;
        }
    }
}
