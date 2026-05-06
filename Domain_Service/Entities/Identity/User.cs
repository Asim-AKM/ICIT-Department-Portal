using Domain_Service.Entities.Academic;
using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Identity
{
    public class User
    {
        [Key]
        public Guid UserId { get; set; }
        public Guid? DepartmentId { get; set; }     
        public string FullName { get; set; }=string.Empty;
        public string UserName { get; set; }=string.Empty;
        public string Email { get; set; }=string.Empty;
        public string Contact { get; set; }=string.Empty;
        public string CNIC { get; set; } = string.Empty;
        public string ImageUrl { get; set; }=string.Empty;
        public UserStatus Status { get; set; }
        public DateTime  CreatedAt { get; set; }
        public Department? Department { get; set; } //  Navigation Property 
        public UserRole? Role { get; set; } //  Navigation Property 
    }
}
