using Skimur.Data.Models;

namespace Skimur.Web.Services
{
    public interface IUserContext
    {
        User CurrentUser { get; }

        bool? CurrentNsfw { get; }
    }
}
