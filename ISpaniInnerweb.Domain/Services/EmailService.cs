using ISpaniInnerweb.Domain.Interfaces;
using ISpaniInnerweb.Domain.Interfaces.Services.Communication;
using ISpaniInnerweb.Domain.Interfaces.Settings;
using MailKit.Net.Smtp;
using MimeKit;
using MimeKit.Text;
using System;
using System.Collections.Generic;
using System.Text;

namespace ISpaniInnerweb.Domain.Services
{
    public class EmailService : IEmailService
    {
        private readonly IEmailSettings emailSettings;
        public EmailService(IEmailSettings emailSettings)
        {
            this.emailSettings = emailSettings;                                                                                                                                                     
        }
        public void SendMail(string subject, string message, string to)
        {
            var emailSettings = this.emailSettings;

            var mailMessage = new MimeMessage();
            mailMessage.From.Add(new MailboxAddress("support@recruitmentJob.co.za", "9999ndamu@gmail.com"));
            mailMessage.To.Add(new MailboxAddress(to, to));
            mailMessage.Subject = subject;
            mailMessage.Body = new TextPart("Plain")
            {
                Text = message
            };

            using (var smtpClient = new SmtpClient())
            {
                smtpClient.Connect("smtp.gmail.com", 587, false);
                smtpClient.Authenticate("9999ndamu@gmail.com", "nDo!nDo0");
                smtpClient.Send(mailMessage);
                smtpClient.Disconnect(true);
            }
        }
    }
}
