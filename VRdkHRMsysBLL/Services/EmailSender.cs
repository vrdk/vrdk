﻿using Microsoft.Extensions.Configuration;
using SendGrid;
using SendGrid.Helpers.Mail;
using System.Threading.Tasks;
using VRdkHRMsysBLL.Interfaces;

namespace VRdkHRMsysBLL.Services
{
    public class EmailSender : IEmailSender
    {
        private readonly SendGridClient _sendGridClient;

        public EmailSender(IConfiguration configurator)
        {
            _sendGridClient = new SendGridClient(configurator["SendGridSettings:Api"]);
        }

        public async Task SendPasswordResetLink(string address, string name, string title, string plainTextContent, string message)
        {
            var from = new EmailAddress("vrdk2019@gmail.com", "vrdk");
            var subject = title;
            var to = new EmailAddress(address, name);
            var htmlContent = "<strong>" + message + "</strong>";
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);
            await _sendGridClient.SendEmailAsync(msg);
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            return Task.CompletedTask;
        }
    }
}
