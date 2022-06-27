using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace Infrastructure
{
    public class MailService : IEmailSender
    {
        private readonly IConfiguration _configuration;
        private readonly SmtpClient _smtpClient;

        public MailService(IConfiguration configuration)
        {
            _configuration = configuration;
            _smtpClient = new SmtpClient();
        }

        public async Task SendEmail(string address, string subject, string htmlMessage)
        {
            try
            {
                var senderAddress = _configuration["MailSettings:Account"];
                var email = new MimeMessage { Sender = new MailboxAddress("MoneyMaster", senderAddress) };
                email.From.Add(new MailboxAddress("MoneyMaster", senderAddress));
                email.To.Add(MailboxAddress.Parse(address));
                email.Subject = subject;

                var builder = new BodyBuilder { HtmlBody = htmlMessage };
                email.Body = builder.ToMessageBody();

                var smtpHost = _configuration["MailSettings:Host"];
                await _smtpClient.ConnectAsync(smtpHost);
                var senderPassword = _configuration["MailSettings:Password"];
                await _smtpClient.AuthenticateAsync(senderAddress, senderPassword);
                await _smtpClient.SendAsync(email);

                await _smtpClient.DisconnectAsync(true);
            }
            catch (Exception ex)
            {
                throw new ApplicationException("Cannot send email");
            }
        }


        public Task BulkSendEmail(List<string> addresses, string subject, string htmlMessage)
        {
            foreach (var address in addresses) SendEmail(address, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}