using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Configuration;
using MimeKit;
using MailKit.Net.Smtp;
using UserManagementSystem.Domain.Repositorys;

namespace UserManagementSystem.Infrastructure.Repositories
{
    public class SendEmailRepository :ISendEmailRepository
    {
        private readonly IConfiguration _config;
        public SendEmailRepository(IConfiguration config)
        {
            _config = config;
        }
        public async Task SendEmailAsync( string subject, string htmlMessage)
        {
            var smtp = _config.GetSection("Smtp");

            var message = new MimeMessage();
            message.From.Add(new MailboxAddress(smtp["FromName"], smtp["FromEmail"]));
            message.To.Add(MailboxAddress.Parse(smtp["FromEmail"]));
            message.Subject = subject;
            message.Body = new BodyBuilder { HtmlBody = htmlMessage }.ToMessageBody();

            using var client = new MailKit.Net.Smtp.SmtpClient();
            await client.ConnectAsync(smtp["Host"], int.Parse(smtp["Port"]), false);
            await client.AuthenticateAsync(smtp["Username"], smtp["Pass"]);
            await client.SendAsync(message);
            await client.DisconnectAsync(true);
        }
    }
}
