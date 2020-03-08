using System;
using System.Threading.Tasks;

namespace Skimur.Web.Services
{
    public interface IEmailSender
    {
        Task SendEmailAsync(string email, string subject, string message);
    }
}
