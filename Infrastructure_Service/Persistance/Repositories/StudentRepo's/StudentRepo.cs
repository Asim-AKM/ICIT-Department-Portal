using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.StudentManagments;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;
using Microsoft.EntityFrameworkCore;

namespace Infrastructure_Service.Persistance.Repositories.StudentRepo_s
{
    public class StudentRepo: Repository<Student>,IStudentRepo
    {
        private readonly ApplicationDbContext _context;
        public StudentRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }

        public async Task<List<Student>> GetStudentListBySessionIdAsync(Guid SessionId)
        {
           return await _context.Students.Where(x=> x.SessionId == SessionId).ToListAsync();
        }

        public async Task<List<string>> StudentRollNoList(Guid sessionId)
        {
            return await _context.Students
                .Where(s => s.SessionId == sessionId)
                .Select(x => x.RollNo)
                .ToListAsync();
        }
    }
}
