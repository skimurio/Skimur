using System.Text.RegularExpressions;
using Skimur.Data.Utils;

namespace Skimur.Data.Services.Impl
{
    public class PasswordManager : IPasswordManager
    {
        /// <summary>
        /// Hashes the password.
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public string HashPassword(string password)
        {
            return Hashing.HashPassword(password);
        }

        /// <summary>
        /// Verifies the hashed password.
        /// </summary>
        /// <param name="hashedPassword">The hashed password.</param>
        /// <param name="providedPassword">The provided password.</param>
        /// <returns></returns>
        public bool VerifyHashedPassword(string hashedPassword, string providedPassword)
        {
            return Hashing.ValidatePassword(providedPassword, hashedPassword);
        }

        /// <summary>
        /// Determines the password strength
        /// </summary>
        /// <param name="password">The password.</param>
        /// <returns></returns>
        public PasswordScore PasswordStrength(string password)
        {
            int score = 1;

            if (password.Length < 1)
                return PasswordScore.Blank;
            if (password.Length < 4)
                return PasswordScore.VeryWeak;

            if (password.Length >= 8)
                score++;
            if (password.Length >= 12)
                score++;
            if (Regex.Match(password, @"d+", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @"[a-z]", RegexOptions.ECMAScript).Success &&
              Regex.Match(password, @"[A-Z]", RegexOptions.ECMAScript).Success)
                score++;
            if (Regex.Match(password, @".[!,@,#,$,%,^,&,*,?,_,~,-,£,(,)]", RegexOptions.ECMAScript).Success)
                score++;

            return (PasswordScore) score;
        }

    }
}
