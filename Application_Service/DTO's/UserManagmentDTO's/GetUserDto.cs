using Domain_Service.Enum;

namespace Application_Service.DTO_s.UserManagmentDTO_s
{
    public class GetUserDto
    {
        public Guid UserId { get; set; }
        public string FullName { get; set; } = string.Empty;
        public string UserName { get; set; } = string.Empty;
        public string Email { get; set; } = string.Empty;
        public string Contact { get; set; } = string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string ImageUrl { get; set; } = string.Empty;
        public string Department { get; set; } = string.Empty;
        public Guid? DepartmentId { get; set; } 
        public RoleType Role { get; set; }
        public UserStatus Status { get; set; }
        public DateTime CreatedAt { get; set; }
    }
}
