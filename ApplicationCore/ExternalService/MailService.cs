using System;
using System.Collections.Generic;
using System.Threading.Tasks;
using ApplicationCore.Interfaces;
using MailKit.Net.Smtp;
using Microsoft.Extensions.Configuration;
using MimeKit;

namespace ApplicationCore.ExternalService
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
            var senderAddress = _configuration["MailSettings:Account"];
            Console.WriteLine(senderAddress);
            var email = new MimeMessage { Sender = new MailboxAddress("MoneyMaster", senderAddress) };
            email.From.Add(new MailboxAddress("MoneyMaster", senderAddress));
            email.To.Add(MailboxAddress.Parse(address));
            email.Subject = subject;

            var builder = new BodyBuilder { HtmlBody = htmlMessage };
            email.Body = builder.ToMessageBody();

            var smtpHost = _configuration["MailSettings:Host"];
            await _smtpClient.ConnectAsync(smtpHost);
            var senderPassword = _configuration["MailSettings:Password"];
            Console.WriteLine(senderPassword);
            await _smtpClient.AuthenticateAsync(senderAddress, senderPassword);
            await _smtpClient.SendAsync(email);

            await _smtpClient.DisconnectAsync(true);
        }



        public Task BulkSendEmail(List<string> addresses, string subject, string htmlMessage)
        {
            foreach (var address in addresses) SendEmail(address, subject, htmlMessage);
            return Task.CompletedTask;
        }
    }
}