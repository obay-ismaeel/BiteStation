using BiteStation.Domain.Abstractions;
using Microsoft.Extensions.Logging;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Text;
using System.Threading.Tasks;

namespace BiteStation.Infrastructure.Services;
public class EmailService : IEmailService
{
    private readonly ILogger<EmailService> _logger;

    public EmailService(ILogger<EmailService> logger)
    {
        _logger = logger;
    }

    public async Task SendEmailAsync(string toEmail, string subject, string body)
    {
        MailMessage mail = new();
        mail.To.Add(new MailAddress(toEmail));
        mail.From = new MailAddress("obay.engineer@gmail.com", "Obay Ismaeel", System.Text.Encoding.UTF8);
        mail.Subject = subject;
        mail.SubjectEncoding = System.Text.Encoding.UTF8;
        mail.Body = body;
        mail.BodyEncoding = System.Text.Encoding.UTF8;
        mail.IsBodyHtml = true;
        mail.Priority = MailPriority.High;

        SmtpClient client = new();
        client.Credentials = new System.Net.NetworkCredential("from gmail address", "your gmail account password");
        client.Port = 587;
        client.Host = "smtp.gmail.com";
        client.EnableSsl = true;

        try
        {
            client.Send(mail);
        }
        catch (Exception ex)
        {
            _logger.LogError("Something went wrong while sending the email");
        }
    }
}
