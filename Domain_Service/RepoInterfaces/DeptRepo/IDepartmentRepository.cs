using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.GenricRepo;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.RepoInterfaces.DeptRepo
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        //Task<List<Department>> GetDepartmentsAsync();
    }
}
