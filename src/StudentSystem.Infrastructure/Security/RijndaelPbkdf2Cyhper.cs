using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

using StudentSystem.Common.Extensions;

namespace StudentSystem.Infrastructure.Security
{
    // https://stackoverflow.com/questions/10168240/encrypting-decrypting-a-string-in-c-sharp
    public class RijndaelPbkdf2Cyhper : ICypher
    {
        private const string Password = "Password";
        private const string Salt = "SaltSalt";

        public string Encrypt(string value)
        {
            var rijndael = new RijndaelManaged();
            using (var passwordKey = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt)))
            {
                rijndael.Key = passwordKey.GetBytes(rijndael.KeySize / 8);
                rijndael.IV = passwordKey.GetBytes(rijndael.BlockSize / 8);
            }

            string result;
            using (var enCryptor = rijndael.CreateEncryptor())
            {
                using (var ms = new MemoryStream())
                {
                    using (var crStream = new CryptoStream(ms, enCryptor, CryptoStreamMode.Write))
                    {
                        var data = Encoding.ASCII.GetBytes(value);
                        crStream.Write(data, 0, data.Length);
                        crStream.Flush();
                        crStream.FlushFinalBlock();
                        result = Convert.ToBase64String(ms.ToArray());
                    }
                }
            }

            return result;
        }

        public string Decrypt(string value)
        {
            var rijndael = new RijndaelManaged();
            using (var passwordKey = new Rfc2898DeriveBytes(Password, Encoding.UTF8.GetBytes(Salt)))
            {
                rijndael.Key = passwordKey.GetBytes(rijndael.KeySize / 8);
                rijndael.IV = passwordKey.GetBytes(rijndael.BlockSize / 8);
            }

            using (var decryptor = rijndael.CreateDecryptor())
            {
                using (var ms = new MemoryStream(Convert.FromBase64String(value)))
                {
                    using (var crStream = new CryptoStream(ms, decryptor, CryptoStreamMode.Read))
                    {
                        string result;
                        using (var sr = new StreamReader(crStream))
                        {
                            result = sr.ReadToEnd();
                        }
                        return result;
                    }
                }
            }
        }

        public bool IsPasswordMatch(string password, string hashedPassword)
        {
            var newHash = Encrypt(password);
            return SlowEquals(newHash.GetUtf8Bytes(), hashedPassword.GetUtf8Bytes());
        }

        private static bool SlowEquals(IReadOnlyList<byte> a, IReadOnlyList<byte> b)
        {
            var diff = (uint)a.Count ^ (uint)b.Count;
            for (var i = 0; i < a.Count && i < b.Count; i++)
                diff |= (uint)(a[i] ^ b[i]);
            return diff == 0;
        }
    }
}