using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Finance
{
    public class Challan
    {
        [Key]
        public Guid ChallanId { get; set; }
        public string ChallanNumber { get; set; } = string.Empty;
        public Guid StudentId { get; set; }
        public Guid SemesterId { get; set; }
        public decimal Amount { get; set; }
        public DateTime DueDate { get; set; }
        public DateTime GeneratedDate { get; set; }
        public ChallanStatus Status { get; set; } // Pending, Paid, Expired
    }
}
