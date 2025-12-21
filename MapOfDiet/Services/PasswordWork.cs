using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace MapOfDiet.Services
{
    public class PasswordWork
    {
        // Генерирует новую рандомную соль
        public static byte[] newSalt()
        {
            byte[] salt = new byte[32];

            using (var rnd = RandomNumberGenerator.Create())
            {
                rnd.GetBytes(salt);
            }
            return salt;
        }

        // Исходя из пароля и соли выдаёт хэш
        public static byte[] newHash(string password, byte[] salt)
        {
            byte[] passwordBytes = Encoding.UTF8.GetBytes(password);
            byte[] combined = new byte[passwordBytes.Length + salt.Length];
            Buffer.BlockCopy(passwordBytes, 0, combined, 0, passwordBytes.Length);
            Buffer.BlockCopy(salt, 0, combined, passwordBytes.Length, salt.Length);
            using (SHA256 sha256 = SHA256.Create())
            {
                return sha256.ComputeHash(combined);
            }
        }
    }
}
