using Domain_Service.RepoInterfaces.StudentManagments;
using Infrastructure_Service.Data;

namespace Infrastructure_Service.Persistance.Repositories.StudentRepo_s
{
    public class StudentRepo: IStudentRepo
    {
        private readonly ApplicationDbContext _context;
        public StudentRepo(ApplicationDbContext context)
        {
            _context = context;
        }
    }
}
