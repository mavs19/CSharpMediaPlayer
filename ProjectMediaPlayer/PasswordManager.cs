using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace ProjectMediaPlayer
{
    public class PasswordManager
    {
        HashComputer m_hashComputer = new HashComputer();

        // Method to generate the hash of the password text and salt value
        public string GeneratePasswordHash(string plainTextPassword, out string salt)
        {
            salt = SaltGenerator.GetSaltString();
            string finalString = plainTextPassword + salt;
            return m_hashComputer.GetPasswordHashAndSalt(finalString);
        }

        // Method to test if the input matches password, returns a boolean result
        public bool IsPasswordMatch(string password, string salt, string hash)
        {
            string finalString = password + salt;
            return hash == m_hashComputer.GetPasswordHashAndSalt(finalString);
        }
    }
}
