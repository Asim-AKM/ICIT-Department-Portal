using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Finance
{
    public class FeeRecord
    {
        [Key]
        public Guid  FeeId { get; set; }
        public Guid  StudentId { get; set; }
        public Guid  SemesterId { get; set; }
        public float Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
