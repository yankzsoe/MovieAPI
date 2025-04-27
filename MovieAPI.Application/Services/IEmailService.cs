using Microsoft.Extensions.Logging;
using Microsoft.Extensions.Options;
using MovieAPI.Application.Common.Models;
using MovieAPI.Application.Common.Models.Configuration;
using MovieAPI.Application.Interfaces;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Polly;

namespace MovieAPI.Application.Services {
    public class EmailService : IEmailService {
        private readonly EmailSettings _emailSettings;
        private readonly ILogger<EmailService> _logger;

        public EmailService(IOptions<EmailSettings> emailSettings
            , ILogger<EmailService> logger) {
            _emailSettings = emailSettings.Value;
            _logger = logger;
        }

        public async Task SendEmailAsync(Email email) {
            await Policy
                .Handle<Exception>()
                .WaitAndRetryAsync(new[]
                {
                    TimeSpan.FromSeconds(3),
                    TimeSpan.FromSeconds(2),
                    TimeSpan.FromSeconds(1)
                }, (ex, timeSpan) => {
                    _logger.LogError("Email service exception: {errorMessage}", ex.InnerException?.Message ?? ex.Message);
                })
                .ExecuteAsync(async () => {
                    using var smtp = new SmtpClient {
                        Host = _emailSettings.SmtpHost,
                        Port = _emailSettings.Port,
                        EnableSsl = _emailSettings.EnableSsl,
                        DeliveryMethod = SmtpDeliveryMethod.Network,
                        Credentials = new NetworkCredential(_emailSettings.Address, _emailSettings.Password),
                        Timeout = _emailSettings.Timeout
                    };

                    var mail = new MailMessage {
                        Subject = email.Subject,
                        From = new MailAddress(_emailSettings.Address, _emailSettings.DisplayName),
                        IsBodyHtml = true,
                        Body = email.Body
                    };

                    foreach (var address in email.Addresses) {
                        mail.To.Add(new MailAddress(address));
                    }

                    await smtp.SendMailAsync(mail);
                });
        }

    }
}
