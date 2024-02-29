using System.Security.Cryptography;

namespace Repositories.Utils.PasswordHasher
{
    public class PasswordHasher : IPasswordHasher
    {
        private const int _saltSize = 16;
        private const int _keySize = 32;
        private const int _iterations = 10000;
        private static readonly HashAlgorithmName _hashAlgorithmName = HashAlgorithmName.SHA256;
        private static char _delimiter = ';';

        public string Hash(string password)
        {
            var salt = RandomNumberGenerator.GetBytes(_saltSize);
            var hash = Rfc2898DeriveBytes.Pbkdf2(password, salt, _iterations, _hashAlgorithmName,_keySize);

            return String.Join(_delimiter, Convert.ToBase64String(salt), Convert.ToBase64String(hash));
        }

        public bool Verify(string storedHashedPassword, string inputPassword)
        {
            var hashElements = storedHashedPassword.Split(_delimiter);
            var salt = Convert.FromBase64String(hashElements[0]);
            var storedHash = Convert.FromBase64String(hashElements[1]);
            var inputHash = Rfc2898DeriveBytes.Pbkdf2(inputPassword, salt, _iterations, _hashAlgorithmName, _keySize);

            return CryptographicOperations.FixedTimeEquals(storedHash, inputHash);
        }
    }
}
