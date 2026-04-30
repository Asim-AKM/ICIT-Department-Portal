using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using Domain_Service.RepoInterfaces.StudentManagments;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;
using System.Linq.Expressions;

namespace Infrastructure_Service.Persistance.Repositories.StudentRepo_s
{
    public class StudentRepo: Repository<Student>,IStudentRepo
    {
        private readonly ApplicationDbContext _context;
        public StudentRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentListBySessionIdAsync(Guid SessionId, StudentStatus studentStatus)
        {
           return await _context.Students.Where(s=> s.SessionId == SessionId && s.Status == studentStatus).ToListAsync();
        }
        public async Task<List<Student>> GetStudentListBySessionIdAsync(Guid SessionId)
        {
            return await _context.Students.Where(s => s.SessionId == SessionId).ToListAsync();
        }

        public async Task<List<Student>> GetStudentsByIdsAsync(List<Guid> ids)
        {
            return await _context.Students.Where(s => ids.Contains(s.StudentId)).ToListAsync();
        }

        //public async Task<List<Student>> GetStudentsByIdsAsync(List<Guid> ids, Expression<Func<Student, Guid>> keySelector)
        //{
        //    return await _context.Students
        //        .Where(e => ids.Contains(keySelector.Compile().Invoke(e)))
        //        .ToListAsync();
        //}
    }
}
