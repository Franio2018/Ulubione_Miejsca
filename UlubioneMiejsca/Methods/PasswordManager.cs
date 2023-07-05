using System.Collections.Specialized;

namespace UlubioneMiejsca.Methods
{
    public class PasswordManager
    {
        public string HashPassword(string password)
        {
            string hashedPassword = BCrypt.Net.BCrypt.HashPassword(password);
            return hashedPassword;
        }
        public bool MatchPassword(string UserPassword, string DbPassword)
        {
            return BCrypt.Net.BCrypt.Verify(UserPassword, DbPassword);
        }
    }
}
