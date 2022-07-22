using Microsoft.Extensions.Logging;
using Ordering.Application.Contracts.Infrastructure;
using Ordering.Application.Helpers.Logging;
using Ordering.Application.Models;
using SendGrid;
using SendGrid.Helpers.Mail;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading;
using System.Threading.Tasks;

namespace Ordering.Infrastructure.Mail
{
    public class EmailService : IEmailService
    {
        private readonly ILogger<EmailService> _logger;
        private readonly EmailSettings _emailSettings;

        public EmailService(
            EmailSettings emailSettings,
            ILogger<EmailService> logger)
        {
            _emailSettings = emailSettings ?? throw new ArgumentNullException($"Argument {nameof(emailSettings)} cannot be null.");
            _logger = logger ?? throw new ArgumentNullException($"Argument {nameof(logger)} cannot be null.");
        }

        public async Task<bool> SendEmailAsync(Email email, CancellationToken cancellationToken = default)
        {
            var client = new SendGridClient(_emailSettings.ApiKey);
            var from = new EmailAddress(_emailSettings.FromAddress, _emailSettings.FromName);
            var subject = email.Subject;
            var to = new EmailAddress(email.To);
            var plainTextContent = email.Body;
            var htmlContent = email.Body;
            var msg = MailHelper.CreateSingleEmail(from, to, subject, plainTextContent, htmlContent);

            UnobtrusiveLoggingHelper.Log(_logger, "Sending email...");

            try
            {
                var response = await client.SendEmailAsync(msg);

                UnobtrusiveLoggingHelper.Log(_logger, response.IsSuccessStatusCode ? "E-mail sent successfully!" : "E-mail sending failure!");

                var responseBody = await response.Body.ReadAsStringAsync(cancellationToken);

                UnobtrusiveLoggingHelper.Log(
                    _logger,
                    "Status code: {StatusCode} | Headers: {@Headers} | Response body {ResponseBody}",
                    args:new object[] { response.Headers, response.StatusCode, responseBody });

                return response.IsSuccessStatusCode;
            }
            catch(Exception exc)
            {
                UnobtrusiveLoggingHelper.Log(
                    _logger,
                    "Error while attempting to send email!",
                    logLevel: LogLevel.Error,
                    exception: exc);

                throw;
            }

        }
    }
}
