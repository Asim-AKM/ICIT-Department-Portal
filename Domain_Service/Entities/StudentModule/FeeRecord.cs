using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.StudentModule
{
    public class FeeRecord
    {
        public Guid  FeeId { get; set; }
        public Guid  StudentId { get; set; }
        public Guid  SemesterId { get; set; }
        public Decimal Amount { get; set; }
        public string Status { get; set; } = string.Empty;
        public DateTime DueDate { get; set; }
    }
}
