using Application_Service.Services.UserManagmentServices.Implementation;
using Domain_Service.Entities.UserManagmentModule;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Application_Service.Mapper_s.UserManagmentMappers
{
    public static class CreadentialMapper
    {
        public static UserCredential MapToCreadDomain(this User user, string password)
        {
            PasswordEncriptor  passwordEncriptor = new PasswordEncriptor();
            passwordEncriptor.CreateHashAndSalt(password, out byte[] salt, out byte[] hash);
            return new UserCredential
            {
                CredentialId = Guid.NewGuid(),
                UserId = user.UserId,
                PasswordHash = hash,
                PasswordSalt= salt,
                OTP = new Random ().Next(100000,999999).ToString(),
                OTPExpiry = DateTime.UtcNow.AddMinutes(5)
            };
        }
    }
}
