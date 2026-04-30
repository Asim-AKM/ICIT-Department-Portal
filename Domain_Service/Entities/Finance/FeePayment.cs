using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Finance
{
    public class FeePayment
    {
        [Key]
        public Guid PaymentId { get; set; }
        public Guid StudentId { get; set; }
        public Guid FeeRecordId { get; set; }
        public decimal AmountPaid { get; set; }
        public DateTime PaymentDate { get; set; }
        public string PaymentMethod { get; set; } = string.Empty; // Cash, Bank Transfer, Credit Card
        public string TransactionId { get; set; } = string.Empty;
        public string ReceiptNumber { get; set; } = string.Empty;
        public PaymentStatus Status { get; set; }
    }
}
