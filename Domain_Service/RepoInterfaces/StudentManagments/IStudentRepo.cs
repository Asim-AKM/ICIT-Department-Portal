using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.GenricRepo;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Domain_Service.RepoInterfaces.StudentManagments
{
    public interface IStudentRepo : IRepository<Student>
    {
        Task<List<Student>> GetStudentListBySessionIdAsync(Guid SessionId);

        //Task<List<Student>> GetStudentsByIdsAsync(List<Guid> ids, Expression<Func<Student, Guid>> keySelector);
        Task<List<Student>> GetStudentsByIdsAsync(List<Guid> ids);


    }
}
