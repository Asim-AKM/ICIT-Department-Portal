using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.DTO_s.FacultyDTO_s
{
    public class GetFacultyDTO
    {
        public Guid FacultyId { get; set; }
        public Guid UserId { get; set; }
        public string FullName { get; set; }
        public Guid DepartmentId { get; set; }
        public string Designation { get; set; } = string.Empty; // Professor, Assistant Professor, etc.
        public DateTime JoiningDate { get; set; }
    }
}
