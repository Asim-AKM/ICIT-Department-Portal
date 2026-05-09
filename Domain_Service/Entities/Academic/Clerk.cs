using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Academic
{
    public class Clerk
    {
        [Key]
        public Guid ClerkId { get; set; }
        public Guid UserId { get; set; }
        public Guid DepartmentId { get; set; }
        public string Designation { get; set; } = string.Empty;
        public DateTime JoiningDate { get; set; }
    }
}
