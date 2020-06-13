using System.Threading.Tasks;
using Skimur.Data.Commands;
using Skimur.Messaging;

namespace Skimur.Web.Services.Impl
{
    public class AuthMessageSender : IEmailSender
    {
        private ICommandBus _commandBus;

        public AuthMessageSender(ICommandBus commandBus)
        {
            _commandBus = commandBus;
        }

        public Task SendEmailAsync(string email, string subject, string message)
        {
            _commandBus.Send(new SendEmail
            {
                Email = email,
                Subject = subject,
                Body = message
            });

            return Task.FromResult(0);
        }

        public Task SendSmsAsync(string number, string message)
        {
            // TODO: create sms provider implementation
            return Task.FromResult(0);
        }
    }
}
