﻿
using System.Net.Mail;
using System.Net;

namespace E_commerce.Utility
{
    public class EmailSender : IEmailSender
    {
        public Task SendEmailAsync(string email, string subject, string message)
        {
            var client = new SmtpClient("smtp.gmail.com", 587)
            {
                EnableSsl = true,
                UseDefaultCredentials = false,
                Credentials = new NetworkCredential("mohamedashrafmahmoudgad@gmail.com", "gcwl ezgb zfoj cpxz")
            };

            return client.SendMailAsync(
                new MailMessage(from: "mohamedashrafmahmoudgad@gmail.com",
                                to: email,
                                subject,
                                message
                                )
                {
                    IsBodyHtml = true
                });
        }
    }
}
