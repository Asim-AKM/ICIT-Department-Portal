using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.SubjectManagmengModels
{
    public class CreateSubjectRequest
    {
        public string Title { get; set; } = string.Empty;

        public Guid DepartmentId { get; set; }

        public Guid SemesterId { get; set; }

        // Optional: can be assigned later
        public Guid? FacultyId { get; set; }

        public int CreditHours { get; set; }
    }
}
