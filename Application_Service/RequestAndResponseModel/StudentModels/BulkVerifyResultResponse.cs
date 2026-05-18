namespace Application_Service.RequestAndResponseModel.StudentModels
{
    public class BulkVerifyResultResponse
    {

        public int Total { get; set; }          // Total students selected
        public int Success { get; set; }        // Successfully verified
        public int Failed { get; set; }         // Failed verifications
        public int skiped { get; set; }
        public List<string> FailedStudents { get; set; } = new(); // Student Names / CNICs
        public List<string> AlreadyVerified { get; set; } = new(); // Already verified students
    }

}
