using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace AdminAssistant.Blog.Helpers
{
    public class Encryption
    {
        public static string Encrypt(string adminUsername, string email, string passwordHash, DateTime created, string secret)
        {
            string theWholeThing = created.ToString() + email + passwordHash + secret + adminUsername + secret;
            byte[] bytes = Encoding.UTF8.GetBytes(theWholeThing);

            SymmetricAlgorithm sa = DES.Create();
            MemoryStream msEncrypt = new MemoryStream();
            CryptoStream csEncrypt = new CryptoStream(msEncrypt, sa.CreateEncryptor(), CryptoStreamMode.Write);
            csEncrypt.Write(bytes, 0, bytes.Length);
            csEncrypt.Close();
            byte[] encryptedTextBytes = msEncrypt.ToArray();
            msEncrypt.Close();
        }

        public static string Decrypt(string stringToDecrypt)
        {
            MemoryStream msDecrypt = new MemoryStream(encryptedTextBytes);
            CryptoStream csDecrypt = new CryptoStream(msDecrypt, sa.CreateDecryptor(), CryptoStreamMode.Read);
            byte[] decryptedTextBytes = new Byte[encryptedTextBytes.Length];
            csDecrypt.Read(decryptedTextBytes, 0, encryptedTextBytes.Length);
            csDecrypt.Close();
            msDecrypt.Close();

            string decryptedTextString = (new UnicodeEncoding()).GetString(decryptedTextBytes);
        }
    }
}
