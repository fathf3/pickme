using Microsoft.Extensions.Configuration;
using PickMe.Business.Services.Abstractions;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Net.Mail;
using System.Net;
using System.Text;
using System.Threading.Tasks;
using Microsoft.Extensions.Options;
using PickMe.Core.ViewModels;

namespace PickMe.Business.Services.Concretes
{
    public class EmailService : IEmailService
    {
        readonly IConfiguration _configuration;

        public EmailService(IConfiguration configuration)
        {
            _configuration = configuration;
        }



        public async Task SendEmailAsync(string to, string subject, string body, bool isBodyHtml = true)
        {
            await SendEmailAsync(new[] { to }, subject, body, isBodyHtml);
        }

        public async Task SendEmailAsync(string[] to, string subject, string body, bool isBodyHtml = true)
        {
            MailMessage mail = new();
            mail.IsBodyHtml = isBodyHtml;
            foreach (var item in to)
            {
                mail.To.Add(item);
            }
            mail.Subject = subject;
            mail.Body = body;
            mail.From = new(_configuration["EMail:Username"], "PickMe Ekibi", System.Text.Encoding.UTF8);

            SmtpClient smtp = new SmtpClient();
            smtp.Credentials = new NetworkCredential(_configuration["EMail:Username"], _configuration["EMail:Password"]);
            smtp.Port = 587;
            smtp.EnableSsl = true;
            smtp.Host = "smtp.gmail.com";
            await smtp.SendMailAsync(mail);
        }


    }
}
