using Domain_Service.Entities.Identity;
using Domain_Service.Enum;
using System.ComponentModel.DataAnnotations;

namespace Domain_Service.Entities.Academic
{
    public class Department
    {
        [Key]
        public Guid DepartmentId { get; set; }

        public string Name { get; set; } = string.Empty;   // BSCS, BBA, etc.

        public string Code { get; set; } = string.Empty;   // CS, SE, IT

        public string Description { get; set; } = string.Empty;

        public DepartmentStatus Status { get; set; }  // Active / Inactive

        public ICollection<User> Users {get; set; } // Navigation property to Users in this department
    }
}
