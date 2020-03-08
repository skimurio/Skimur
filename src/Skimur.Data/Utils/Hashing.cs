namespace Skimur.Data.Utils
{
    using BCrypt.Net;

    public static class Hashing
    {
        private static string GetRandomSalt()
        {
            return BCrypt.GenerateSalt(12);
        }

        public static string HashPassword(string password)
        {
            return BCrypt.HashPassword(password, GetRandomSalt());
        }

        public static bool ValidatePassword(string password, string correctHash)
        {
            return BCrypt.Verify(password, correctHash);
        }
    }
}
