using Application.Services.Interfaces;
using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Threading;
using System.Threading.Tasks;

namespace Application.Services.Implementation
{
    public class EmailService : IEmailService
    {
        private readonly string _apiKey;
        private readonly string _fromEmail;
        private readonly string _fromName;

        public EmailService(IConfiguration configuration)
        {
            _apiKey = configuration["SendGrid:ApiKey"] ?? throw new ArgumentNullException("SendGrid:ApiKey");
            _fromEmail = configuration["SendGrid:FromEmail"] ?? throw new ArgumentNullException("SendGrid:FromEmail");
            _fromName = configuration["SendGrid:FromName"] ?? throw new ArgumentNullException("SendGrid:FromName");
        }

        public async Task<bool> SendOtpEmailAsync(string toEmail, string userName, string otp, CancellationToken cancellationToken = default)
        {
            var client = new SendGridClient(_apiKey);
            var from = new EmailAddress(_fromEmail, _fromName);
            var to = new EmailAddress(toEmail, userName);
            var subject = "Reset Your Password - Decortee System";

            var htmlContent = $@"
<!DOCTYPE html>
<html>
<head>
    <style>
        body {{
            font-family: Arial, sans-serif;
            background-color: #f4f4f4;
            margin: 0;
            padding: 0;
        }}
        .container {{
            max-width: 600px;
            margin: 50px auto;
            background-color: #ffffff;
            padding: 30px;
            border-radius: 10px;
            box-shadow: 0 4px 6px rgba(0, 0, 0, 0.1);
        }}
        .header {{
            text-align: center;
            padding-bottom: 20px;
            border-bottom: 2px solid #4CAF50;
        }}
        .header h1 {{
            color: #4CAF50;
            margin: 0;
        }}
        .content {{
            padding: 30px 0;
            text-align: center;
        }}
        .content p {{
            font-size: 16px;
            color: #555;
            line-height: 1.6;
        }}
        .otp-box {{
            display: inline-block;
            background-color: #4CAF50;
            color: #ffffff;
            font-size: 32px;
            font-weight: bold;
            padding: 20px 40px;
            border-radius: 8px;
            letter-spacing: 8px;
            margin: 20px 0;
        }}
        .footer {{
            text-align: center;
            padding-top: 20px;
            border-top: 2px solid #e0e0e0;
            color: #999;
            font-size: 12px;
        }}
        .warning {{
            background-color: #fff3cd;
            border: 1px solid #ffeaa7;
            border-radius: 5px;
            padding: 15px;
            margin: 20px 0;
            color: #856404;
        }}
    </style>
</head>
<body>
    <div class='container'>
        <div class='header'>
            <h1>🔐 Decortee System</h1>
        </div>
        <div class='content'>
            <p>Hello <strong>{userName}</strong>,</p>
            <p>We received a request to reset your password. Use the OTP below to reset your password:</p>
            <div class='otp-box'>{otp}</div>
            <p>This code will expire in <strong>10 minutes</strong>.</p>
            <div class='warning'>
                <strong>⚠️ Security Notice:</strong> If you didn't request this password reset, please ignore this email and ensure your account is secure.
            </div>
        </div>
        <div class='footer'>
            <p>&copy; 2025 Decortee System. All rights reserved.</p>
            <p>This is an automated email. Please do not reply to this message.</p>
        </div>
    </div>
</body>
</html>";

            var plainTextContent = $@"
Hello {userName},

We received a request to reset your password.

Your OTP: {otp}

This code will expire in 10 minutes.

If you didn't request this password reset, please ignore this email.

© 2025 Decortee System. All rights reserved.
";

            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            var response = await client.SendEmailAsync(msg, cancellationToken);

            return response.IsSuccessStatusCode;
        }
    }
}