using Domain_Service.Entities.Academic;
using Domain_Service.RepoInterfaces.GenricRepo;

namespace Domain_Service.RepoInterfaces.DeptRepo
{
    public interface IDepartmentRepository : IRepository<Department>
    {
        //Task<List<Department>> GetDepartmentsAsync();
    }
}
