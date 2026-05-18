using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.RequestAndResponseModel.GradeManagmentModels
{
    public class CreateOrUpdateGradeRequest
    {
        public Guid EnrollmentId { get; set; }
        public int MidtermMarks { get; set; }
        public int FinalMarks { get; set; }
        public int AssignmentMarks { get; set; }
        public int QuizMarks { get; set; }
    }   
}
