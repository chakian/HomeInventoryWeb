using HomeInv.Common.Configuration;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.IO;
using System.Threading.Tasks;

namespace HomeInv.Business.Services;

public class EmailSenderService : IEmailSenderService
{
    private readonly EmailSenderOptions _emailSenderOptions;
    public EmailSenderService(IOptions<EmailSenderOptions> emailSenderOptions)
    {
        _emailSenderOptions = emailSenderOptions.Value;
    }
    // TODO: Sample: https://codewithmukesh.com/blog/send-emails-with-aspnet-core/
    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.Sender = MailboxAddress.Parse(_emailSenderOptions.SenderAddress);
            foreach (var toEmail in mailRequest.ToEmailList)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            }
            emailMessage.Subject = mailRequest.Subject;
            var builder = new BodyBuilder();
            if (mailRequest.Attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in mailRequest.Attachments)
                {
                    if (file.Length > 0)
                    {
                        using (var ms = new MemoryStream())
                        {
                            file.CopyTo(ms);
                            fileBytes = ms.ToArray();
                        }
                        builder.Attachments.Add(file.FileName, fileBytes, MimeKit.ContentType.Parse(file.ContentType));
                    }
                }
            }
            builder.HtmlBody = mailRequest.Body;
            emailMessage.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSenderOptions.SmtpServer, _emailSenderOptions.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSenderOptions.SenderAddress, _emailSenderOptions.SenderPassword);
            await smtp.SendAsync(emailMessage);
            smtp.Disconnect(true);
        }
        catch(Exception ex)
        {
            string error = ex.Message;
        }
    }
}
