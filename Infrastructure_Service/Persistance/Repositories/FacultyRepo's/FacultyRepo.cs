using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.FacultyRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.FacultyRepo_s
{
    public class FacultyRepo : Repository<Faculty>, IFacultyRepo
    {
        private readonly ApplicationDbContext _context;
        public FacultyRepo(ApplicationDbContext context ): base( context )
        {
            _context = context;
        }
    }
}
