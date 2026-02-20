using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.FacultyModule
{
    public class Subject
    {
        public  Guid SubjectId { get; set; }
        public  String Title { get; set; }=String.Empty;
        public  Guid SemesterId{ get; set; }
        public  Guid FacultyId{ get; set; }
    }
}
