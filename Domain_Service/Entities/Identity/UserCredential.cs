using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.Identity
{
    public class UserCredential
    {
        [Key]
        public Guid CredentialId { get; set; }
        public Guid UserId { get; set; }
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
    }
}
