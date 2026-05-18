using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Academic
{
    public class Subject
    {
        [Key]
        public  Guid SubjectId { get; set; }
        public  string Title { get; set; }=String.Empty;
        public Guid DepartmentId { get; set; }
        public  Guid SemesterId{ get; set; }
        public  Guid? FacultyId{ get; set; }
        public int CreditHours { get; set; }
        public bool IsActive { get; set; } = true;

        public Semester Semester { get; set; } // Navigational Property
        public Department Department { get; set; } // Navigational Property

        public Faculty Faculty { get; set; } // Navigational Property
    }
}
