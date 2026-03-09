using Domain_Service.RepoInterfaces.EmailRepo;
using MailKit.Net.Smtp;
using MailKit.Security;
using MimeKit;
namespace Infrastructure_Service.Persistance.Repositories.EmailRepo_s
{
    public class EmailRepository : IEmailRepository
    {
        private static readonly string _smtpServer = "smtp.gmail.com";
        private static readonly int _port = 465;
        private static readonly string _fromEmail = "asimkhanii7777@gmail.com";
        private static readonly string _password = "gcsf lgow rkrh laxf";
        private static readonly string _fromName = "ICIT Department";

        public async Task<bool> SendStudentVerificationEmail(string toEmail, string studentName, string cnic, string tempPassword)
        {
            #region HtmlBodyDesing
            var subject = "Student Account Verified - ICIT Department";
            var HTMlbody = @"<!DOCTYPE html>
                    <html>
                    <head>
                    <meta charset=""UTF-8"">
                    <style>
                    body {
                        margin: 0;
                        padding: 0;
                        background-color: #f0f2f5;
                        font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
                    }

                    .container {
                        max-width: 650px;
                        margin: 50px auto;
                        background: #ffffff;
                        border-radius: 15px;
                        overflow: hidden;
                        box-shadow: 0 8px 25px rgba(0,0,0,0.15);
                        border-top: 8px solid #4e73df;
                    }

                    .header {
                        background: linear-gradient(135deg, #4e73df, #1cc88a);
                        color: #ffffff;
                        text-align: center;
                        padding: 30px 20px;
                    }

                    .header h1 {
                        margin: 0;
                        font-size: 28px;
                    }

                    .header p {
                        margin: 5px 0 0 0;
                        font-size: 16px;
                        opacity: 0.9;
                    }

                    .content {
                        padding: 30px;
                        color: #333333;
                        line-height: 1.6;
                    }

                    .content p {
                        font-size: 16px;
                        margin: 15px 0;
                    }

                    .credentials {
                        background: #f8f9fc;
                        border: 1px solid #e3e6f0;
                        border-radius: 10px;
                        padding: 20px;
                        margin: 20px 0;
                    }

                    .credentials table {
                        width: 100%;
                    }

                    .credentials td {
                        padding: 10px 0;
                    }

                    .label {
                        font-weight: bold;
                        color: #4e73df;
                        width: 40%;
                    }

                    .value {
                        font-weight: bold;
                        color: #1cc88a;
                    }

                    .button {
                        display: inline-block;
                        margin-top: 20px;
                        padding: 14px 30px;
                        background: linear-gradient(135deg, #1cc88a, #36b9cc);
                        color: #ffffff !important;
                        text-decoration: none;
                        border-radius: 50px;
                        font-weight: bold;
                        box-shadow: 0 4px 12px rgba(0,0,0,0.2);
                        transition: all 0.3s ease;
                    }

                    .button:hover {
                        transform: translateY(-2px);
                        box-shadow: 0 6px 18px rgba(0,0,0,0.3);
                    }

                    .footer {
                        background: #f1f3f6;
                        text-align: center;
                        padding: 20px;
                        font-size: 13px;
                        color: #888888;
                    }
                    </style>
                    </head>
                    <body>

                    <div class=""container"">

                    <div class=""header"">
                    <h1>🎓 ICIT Department</h1>
                    <p>Student Portal Notification</p>
                    </div>

                    <div class=""content"">
                    <p>Dear <b>{StudentName}</b>,</p>

                    <p>Congratulations! Your student account has been <b>verified by the admin</b>. You can now login to the portal using the credentials below:</p>

                    <div class=""credentials"">
                    <table>
                    <tr>
                    <td class=""label"">CNIC</td>
                    <td class=""value"">{CNIC}</td>
                    </tr>
                    <tr>
                    <td class=""label"">Temporary Password</td>
                    <td class=""value"">{TempPassword}</td>
                    </tr>
                    </table>
                    </div>

                    <p>⚠ For security reasons, please change your password immediately after first login.</p>

                    </div>

                    <div class=""footer"">
                    © 2026 ICIT Department. This is an automated message. Please do not reply.
                    </div>

                    </div>

                    </body>
                    </html>";

            #endregion

            var body = HTMlbody
                .Replace("{StudentName}", studentName)
                .Replace("{TempPassword}", tempPassword)
                .Replace("{CNIC}", cnic);
            return await SendEmailAsync(toEmail, subject, body);
        }



        public async Task<bool> SendEmailAsync(string toEmail, string subject, string body)
        {
            try
            {
                var message = new MimeMessage();
                message.From.Add(new MailboxAddress("ICIT Department", _fromEmail));
                message.To.Add(MailboxAddress.Parse(toEmail));
                message.Subject = subject;
                message.Body = new TextPart("html") { Text = body };

                using var client = new SmtpClient();

                // USE PORT 465 (SSL)
                await client.ConnectAsync(_smtpServer, 465, SecureSocketOptions.SslOnConnect);

                await client.AuthenticateAsync(_fromEmail, _password);
                await client.SendAsync(message);
                await client.DisconnectAsync(true);

                return true;
            }
            catch (Exception ex)
            {
                Console.WriteLine("Email sending failed: " + ex.Message);
                return false;
            }
        }
    }

}

