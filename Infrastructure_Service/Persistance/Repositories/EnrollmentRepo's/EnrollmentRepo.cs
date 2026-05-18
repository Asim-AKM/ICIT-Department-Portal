using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.EnrollmentRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.EnrollmentRepo_s
{
    public class EnrollmentRepo : Repository<Enrollment> , IEnrollmentRepo
    {
        ApplicationDbContext context;
        public EnrollmentRepo(ApplicationDbContext dbContext) : base(dbContext)
        {
            context = dbContext;
        }
    }
}
