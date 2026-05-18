using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.ResultLockedRepo;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Infrastructure_Service.Persistance.Repositories.ResultLockRepo_s
{
    public class ResultLockRepo : Repository<ResultLock>, IResultLockRepo
    {
        private readonly ApplicationDbContext dbContext;
        public ResultLockRepo(ApplicationDbContext context) :  base(context)
        {
            dbContext = context;
        }
    }
}
