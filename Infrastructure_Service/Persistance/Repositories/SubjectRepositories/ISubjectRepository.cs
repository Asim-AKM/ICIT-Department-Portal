using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.SubjectRepo_s;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.SubjectRepositories
{
    public class SubjectRepository : Repository<Subject> , ISubjectRepository
    {
        ApplicationDbContext context;
        public SubjectRepository(ApplicationDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }
    }
}
