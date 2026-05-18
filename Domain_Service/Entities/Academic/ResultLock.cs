using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Academic
{
    public class ResultLock
    {
        [Key]
        public Guid ResultLockId { get; set; }

        public Guid SemesterId { get; set; }

        public Guid DepartmentId { get; set; }

        public bool IsLocked { get; set; } = false;

        public DateTime? LockedAt { get; set; }

        public Guid? LockedBy { get; set; }

        public string Remarks { get; set; } = string.Empty;
    }
}
