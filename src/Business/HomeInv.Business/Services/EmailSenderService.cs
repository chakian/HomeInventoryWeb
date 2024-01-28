using HomeInv.Common.Configuration;
using HomeInv.Common.Interfaces.Services;
using HomeInv.Common.Models;
using MailKit.Net.Smtp;
using MailKit.Security;
using Microsoft.AspNetCore.Http;
using Microsoft.AspNetCore.Identity.UI.Services;
using Microsoft.Extensions.Options;
using MimeKit;
using System;
using System.Collections.Generic;
using System.IO;
using System.Threading.Tasks;

namespace HomeInv.Business.Services;

public class EmailSenderService : IEmailSenderService, IEmailSender
{
    private readonly EmailSenderOptions _emailSenderOptions;
    public EmailSenderService(IOptions<EmailSenderOptions> emailSenderOptions)
    {
        _emailSenderOptions = emailSenderOptions.Value;
    }
    // TODO: Sample: https://codewithmukesh.com/blog/send-emails-with-aspnet-core/
    public async Task SendEmailAsync(MailRequest mailRequest)
    {
        await SendEmailAsync(
            mailRequest.ToEmailList,
            mailRequest.Subject,
            mailRequest.Body,
            mailRequest.Attachments);
    }

    public async Task SendEmailAsync(string email, string subject, string htmlMessage)
    {
        await SendEmailAsync(
            new List<string>() { email }, 
            subject, 
            htmlMessage);
    }

    private async Task SendEmailAsync(IList<string> recipientList,
        string subject,
        string body,
        IList<IFormFile> attachments = null)
    {
        try
        {
            var emailMessage = new MimeMessage();
            emailMessage.Sender = MailboxAddress.Parse(_emailSenderOptions.SenderAddress);
            foreach (var toEmail in recipientList)
            {
                emailMessage.To.Add(MailboxAddress.Parse(toEmail));
            }
            emailMessage.Subject = subject;
            var builder = new BodyBuilder();
            if (attachments != null)
            {
                byte[] fileBytes;
                foreach (var file in attachments)
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
            builder.HtmlBody = body;
            emailMessage.Body = builder.ToMessageBody();
            using var smtp = new SmtpClient();
            smtp.Connect(_emailSenderOptions.SmtpServer, _emailSenderOptions.SmtpPort, SecureSocketOptions.StartTls);
            smtp.Authenticate(_emailSenderOptions.SenderAddress, _emailSenderOptions.SenderPassword);
            await smtp.SendAsync(emailMessage);
            smtp.Disconnect(true);
        }
        catch (Exception ex)
        {
            string error = ex.Message;
        }
    }
}
