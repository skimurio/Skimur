using System.Threading.Tasks;

namespace Skimur.Web.Services
{
    public interface ISmsSender
    {
        Task SendSmsAsync(string number, string message);
    }
}
