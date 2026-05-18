namespace Application_Service.DTO_s.SubjectDTO_s
{
    public class GetSubjectDto
    {
        public Guid SubjectId { get; set; }
        public string Title { get; set; } = string.Empty;
        public Guid DepartmentId { get; set; }
        public string DepartmentName { get; set; } = string.Empty;
        public Guid SemesterId { get; set; }
        public string SemesterName { get; set; } = string.Empty;
        public Guid? FacultyId { get; set; }
        public string FacultyName { get; set; } = string.Empty;
        public int CreditHours { get;set;  }
        public bool IsActive { get; set; }
    }
}
