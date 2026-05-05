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
        public async Task<bool> SendAccountCreatWelcomeEmail(string toEmail, string fullName, string username, string tempPassword, string role)
        {
            var subject = "🎉 Welcome to ICIT Portal - Account Created Successfully";

            #region HTMLBody
            var htmlBody = @"<!DOCTYPE html>
<html>
<head>
    <meta charset=""UTF-8"">
    <style>
        body {
            margin: 0;
            padding: 0;
            background: linear-gradient(135deg, #0f172a, #1e293b);
            font-family: 'Segoe UI', Tahoma, Geneva, Verdana, sans-serif;
        }
        .container {
            max-width: 600px;
            margin: 40px auto;
            background: #ffffff;
            border-radius: 24px;
            overflow: hidden;
            box-shadow: 0 25px 60px rgba(0,0,0,0.3);
        }
        .header {
            background: linear-gradient(135deg, #059669, #10b981);
            color: #ffffff;
            text-align: center;
            padding: 40px 30px;
            position: relative;
            overflow: hidden;
        }
        .header::before {
            content: '';
            position: absolute;
            top: -50%;
            right: -20%;
            width: 200px;
            height: 200px;
            background: rgba(255,255,255,0.08);
            border-radius: 50%;
        }
        .header::after {
            content: '';
            position: absolute;
            bottom: -30%;
            left: -10%;
            width: 150px;
            height: 150px;
            background: rgba(255,255,255,0.05);
            border-radius: 50%;
        }
        .header-icon {
            width: 70px;
            height: 70px;
            background: rgba(255,255,255,0.2);
            border-radius: 50%;
            display: inline-flex;
            align-items: center;
            justify-content: center;
            font-size: 35px;
            margin-bottom: 15px;
            backdrop-filter: blur(10px);
            border: 2px solid rgba(255,255,255,0.3);
        }
        .header h1 {
            margin: 0;
            font-size: 28px;
            font-weight: 800;
            letter-spacing: -0.5px;
            position: relative;
            z-index: 1;
        }
        .header p {
            margin: 8px 0 0 0;
            font-size: 14px;
            opacity: 0.9;
            position: relative;
            z-index: 1;
        }
        .wave {
            background: #ffffff;
            height: 40px;
            position: relative;
        }
        .wave::before {
            content: '';
            position: absolute;
            top: -20px;
            left: 0;
            right: 0;
            height: 40px;
            background: #ffffff;
            border-radius: 50% 50% 0 0;
        }
        .content {
            padding: 30px 35px;
            color: #334155;
            line-height: 1.8;
        }
        .welcome-text {
            font-size: 17px;
            margin-bottom: 20px;
            color: #1e293b;
            border-left: 4px solid #10b981;
            padding-left: 15px;
        }
        .info-card {
            background: linear-gradient(135deg, #f0fdf4, #ecfdf5);
            border: 1px solid #a7f3d0;
            border-radius: 16px;
            padding: 25px;
            margin: 25px 0;
        }
        .info-card h3 {
            margin: 0 0 15px 0;
            color: #059669;
            font-size: 16px;
            text-transform: uppercase;
            letter-spacing: 1px;
        }
        .info-row {
            display: flex;
            justify-content: space-between;
            padding: 10px 0;
            border-bottom: 1px dashed #d1fae5;
        }
        .info-row:last-child {
            border-bottom: none;
        }
        .info-label {
            font-weight: 600;
            color: #047857;
            font-size: 13px;
            text-transform: uppercase;
            letter-spacing: 0.5px;
        }
        .info-value {
            font-weight: 700;
            color: #064e3b;
            font-size: 14px;
        }
        .password-box {
            background: #fef3c7;
            border: 2px dashed #f59e0b;
            border-radius: 12px;
            padding: 15px 20px;
            text-align: center;
            margin: 20px 0;
        }
        .password-box .pass {
            font-size: 26px;
            font-weight: 800;
            color: #d97706;
            letter-spacing: 3px;
            font-family: 'Courier New', monospace;
        }
        .btn {
            display: inline-block;
            margin-top: 15px;
            padding: 15px 35px;
            background: linear-gradient(135deg, #059669, #10b981);
            color: #ffffff !important;
            text-decoration: none;
            border-radius: 50px;
            font-weight: 700;
            font-size: 15px;
            box-shadow: 0 8px 25px rgba(5, 150, 105, 0.3);
            transition: all 0.3s ease;
        }
        .btn:hover {
            transform: translateY(-3px);
            box-shadow: 0 12px 35px rgba(5, 150, 105, 0.4);
        }
        .warning-box {
            background: #fff7ed;
            border: 1px solid #fed7aa;
            border-radius: 12px;
            padding: 15px 20px;
            margin: 20px 0;
            display: flex;
            align-items: flex-start;
            gap: 12px;
        }
        .warning-icon {
            font-size: 22px;
            flex-shrink: 0;
        }
        .warning-text {
            font-size: 13px;
            color: #c2410c;
            margin: 0;
        }
        .footer {
            background: #f8fafc;
            text-align: center;
            padding: 25px;
            border-top: 1px solid #e2e8f0;
        }
        .footer p {
            margin: 3px 0;
            color: #94a3b8;
            font-size: 12px;
        }
        .footer .brand {
            font-weight: 700;
            color: #059669;
            font-size: 14px;
        }
    </style>
</head>
<body>

<div class=""container"">

    <!-- Header -->
    <div class=""header"">
        <div class=""header-icon"">🎓</div>
        <h1>Welcome to ICIT Portal!</h1>
        <p>Gomal University, Dera Ismail Khan</p>
    </div>

    <div class=""wave""></div>

    <!-- Content -->
    <div class=""content"">
        
        <div class=""welcome-text"">
            Dear <b>{FullName}</b>,<br>
            Your account has been created successfully. Welcome aboard! 🚀
        </div>

        <p style=""color:#64748b; font-size:14px;"">
            You have been registered as <b style=""color:#059669;"">{Role}</b> in the ICIT Portal. 
            Below are your account credentials:
        </p>

        <!-- Account Info Card -->
        <div class=""info-card"">
            <h3>📋 Account Details</h3>
            <div class=""info-row"">
                <span class=""info-label"">Full Name : </span>
                <span class=""info-value"">{FullName}</span>
            </div>
            <div class=""info-row"">
                <span class=""info-label"">Username : </span>
                <span class=""info-value"">{UserName}</span>
            </div>
            <div class=""info-row"">
                <span class=""info-label"">Email : </span>
                <span class=""info-value"">{Email}</span>
            </div>
            <div class=""info-row"">
                <span class=""info-label"">Role : </span>
                <span class=""info-value"">{Role}</span>
            </div>
        </div>

        <!-- Temporary Password -->
        <div class=""password-box"">
            <p style=""margin:0 0 8px 0; font-size:13px; color:#92400e; font-weight:600;"">🔑 TEMPORARY PASSWORD</p>
            <span class=""pass"">{TempPassword}</span>
        </div>

        <!-- Warning -->
        <div class=""warning-box"">
            <span class=""warning-icon"">⚠️</span>
            <p class=""warning-text"">
                For security reasons, please <b>change your password immediately</b> after your first login. 
                This temporary password will expire soon.
            </p>
        </div>

        <!-- CTA Button -->
        <div style=""text-align:center;"">
            <a href=""http://localhost:4200/login"" class=""btn"">
                🔗 Go to Login Portal
            </a>
        </div>

        <p style=""text-align:center; margin-top:20px; color:#94a3b8; font-size:12px;"">
            Or visit: <b>http://localhost:4200/login</b>
        </p>

    </div>

    <!-- Footer -->
    <div class=""footer"">
        <p class=""brand"">ICIT Department</p>
        <p>Gomal University, Dera Ismail Khan, KPK</p>
        <p style=""margin-top:8px;"">© {CurrentYear} ICIT Portal. All rights reserved.</p>
        <p>This is an automated message. Please do not reply to this email.</p>
    </div>

</div>

</body>
</html>";
            #endregion

            var body = htmlBody
                .Replace("{FullName}", fullName)
                .Replace("{UserName}", username)
                .Replace("{Email}", toEmail)
                .Replace("{Role}", role)
                .Replace("{TempPassword}", tempPassword)
                .Replace("{CurrentYear}", DateTime.UtcNow.Year.ToString());

            return await SendEmailAsync(toEmail, subject, body);
        }
    }

}

