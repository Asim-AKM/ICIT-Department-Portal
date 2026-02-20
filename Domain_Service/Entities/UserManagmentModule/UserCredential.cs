using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.Entities.UserManagmentModule
{
    public class UserCredential
    {
        public Guid CredentialId { get; set; }
        public Guid UserId { get; set; }
        public byte[] PasswordHash { get; set; } = Array.Empty<byte>();
        public byte[] PasswordSalt { get; set; } = Array.Empty<byte>();
        public string? OTP { get; set; }
        public DateTime? OTPExpiry { get; set; }
    }
}
