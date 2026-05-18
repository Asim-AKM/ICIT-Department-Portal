namespace Application_Service.DTO_s.SemesterDTO_s
{
    public class GetSemeterDto
    {
        public Guid SemesterId { get; set; }

        public Guid SessionId { get; set; }

        public string Name { get; set; } = string.Empty; // "Semester 1"

        public int Order { get; set; } // 1 to 8 using for sorting and display purposes

        public int AcademicYear { get; set; } // 1 to 4 (very important for BS)

        public DateTime StartDate { get; set; }

        public DateTime EndDate { get; set; }
    }
}
