using System.Security.Cryptography;

namespace ProjectMediaPlayer
{
    class HashComputer
    {
        // Method to generate and return the passwaord hash using SHA256
        public string GetPasswordHashAndSalt(string message)
        {
            // Let us use SHA256 algorithm to
            // generate the hash from this salted password
            SHA256 sha = new SHA256CryptoServiceProvider();
            byte[] dataBytes = Utility.GetBytes(message);
            byte[] resultBytes = sha.ComputeHash(dataBytes);
            // return the hash string to the caller
            return Utility.GetString(resultBytes);
        }
    }
}
