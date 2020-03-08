using Microsoft.AspNetCore.Identity;

namespace Skimur.Web.Infrastructure.Identity
{
    public class ApplicationLookupNormalizer : ILookupNormalizer
    {

        public string NormalizeEmail(string email)
        {
            // we don't need to normalize
            return email;
        }

        public string NormalizeName(string name)
        {
            // we don't need to normalize
            return name;
        }
    }
}
