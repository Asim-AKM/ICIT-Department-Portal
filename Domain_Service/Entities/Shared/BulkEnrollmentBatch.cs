using Domain_Service.Enum;
using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Shared
{
    public class BulkEnrollmentBatch
    {
        [Key]
        public Guid BatchId { get; set; }
        public string FileName { get; set; } = string.Empty;
        public int TotalRecords { get; set; }
        public int SuccessfulRecords { get; set; }
        public int FailedRecords { get; set; }
        public Guid UploadedBy { get; set; }
        public DateTime UploadedDate { get; set; }
        public BatchStatus Status { get; set; }
    }
}
