using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Domain_Service.RepoInterfaces.EmailRepo
{
    public interface IEmailRepository
    {
        Task<bool> SendStudentVerificationEmail(string toEmail, string studentName, string cnic, string tempPassword);
        Task<bool> SendEmailAsync(string toEmail, string subject, string body);
        Task<bool> SendAccountCreatWelcomeEmail(string toEmail, string fullName, string username, string tempPassword, string role);
    }
}
