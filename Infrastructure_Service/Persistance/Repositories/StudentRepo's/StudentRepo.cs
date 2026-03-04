using Domain_Service.Entities.StudentModule;
using Domain_Service.RepoInterfaces.StudentManagments;
using Infrastructure_Service.Data;
using Infrastructure_Service.Persistance.GenericRepository;

namespace Infrastructure_Service.Persistance.Repositories.StudentRepo_s
{
    public class StudentRepo: Repository<Student>,IStudentRepo
    {
        private readonly ApplicationDbContext _context;
        public StudentRepo(ApplicationDbContext context) : base(context)
        {
            _context = context;
        }
    }
}
