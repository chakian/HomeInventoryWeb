using HomeInv.Common.Models;
using System.Threading.Tasks;

namespace HomeInv.Common.Interfaces.Services;

public interface IEmailSenderService
{
    Task SendEmailAsync(MailRequest mailRequest);
}
