using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

namespace Findes.Standard.Core.Util
{
    public class CryptographyHelper
    {
        static string keyString = "E546C8DF278CD5931069B522E695D4F2";
        static List<char> invalidChars = new List<char>() { '\\', '/', '#', '+', '-', '!', '$', '@', '%', '&', '^' };

        public static string Encrypt(string text) {
            var characterException = "";
            int stringPos = 0;
            foreach (char c in text) {
                if (invalidChars.Contains(c)) {
                    characterException += stringPos.ToString() + c.ToString() + "_";
                }
                stringPos++;
            }
            foreach (var c in invalidChars) {
                text = text.Replace(c.ToString(), "");
            }

            string encryptedValue = "";

            try {
                var key = Encoding.UTF8.GetBytes(keyString);

                using (var aesAlg = Aes.Create()) {
                    using (var encryptor = aesAlg.CreateEncryptor(key, aesAlg.IV)) {
                        using (var msEncrypt = new MemoryStream()) {
                            using (var csEncrypt = new CryptoStream(msEncrypt, encryptor, CryptoStreamMode.Write))
                            using (var swEncrypt = new StreamWriter(csEncrypt)) {
                                swEncrypt.Write(text);
                            }

                            var iv = aesAlg.IV;

                            var decryptedContent = msEncrypt.ToArray();

                            var result = new byte[iv.Length + decryptedContent.Length];

                            Buffer.BlockCopy(iv, 0, result, 0, iv.Length);
                            Buffer.BlockCopy(decryptedContent, 0, result, iv.Length, decryptedContent.Length);

                            encryptedValue = Convert.ToBase64String(result);
                            if (!string.IsNullOrEmpty(characterException)) {
                                encryptedValue = encryptedValue + "__" + characterException;
                            }
                            return encryptedValue;
                        }
                    }
                }
            }
            catch (Exception ex) {
            }

            return encryptedValue;
        }

        public static string Decrypt(string cipherText) {
            var decryptedValue = "";
            var characterException = "";

            try {
                if (cipherText.IndexOf("=__") > 0) {
                    characterException = cipherText.Substring(cipherText.IndexOf("=__") + 3);
                    cipherText = cipherText.Substring(0, cipherText.LastIndexOf("=") + 1);
                }

                var fullCipher = Convert.FromBase64String(cipherText);

                var iv = new byte[16];
                var cipher = new byte[16];

                Buffer.BlockCopy(fullCipher, 0, iv, 0, iv.Length);
                Buffer.BlockCopy(fullCipher, iv.Length, cipher, 0, iv.Length);
                var key = Encoding.UTF8.GetBytes(keyString);

                using (var aesAlg = Aes.Create()) {
                    using (var decryptor = aesAlg.CreateDecryptor(key, iv)) {
                        string result;
                        using (var msDecrypt = new MemoryStream(cipher)) {
                            using (var csDecrypt = new CryptoStream(msDecrypt, decryptor, CryptoStreamMode.Read)) {
                                using (var srDecrypt = new StreamReader(csDecrypt)) {
                                    result = srDecrypt.ReadToEnd();
                                }
                            }
                        }

                        if (!string.IsNullOrEmpty(characterException)) {
                            var sb = new StringBuilder(result);
                            foreach (var item in characterException.Split(new char[] { '_' }, StringSplitOptions.RemoveEmptyEntries)) {
                                var position = int.Parse(Regex.Match(item, @"\d+").Value);
                                var charToImcrement = item.Replace(position.ToString(), "");
                                sb.Insert(position, charToImcrement[0]);
                            }
                            result = sb.ToString();
                        }

                        return result;
                    }
                }
            }
            catch (Exception ex) {
            }

            return decryptedValue;
        }

        public static string HashPassword(string password) {
            byte[] salt;
            byte[] buffer2;
            if (password == null) {
                throw new ArgumentNullException("password");
            }
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, 0x10, 0x3e8)) {
                salt = bytes.Salt;
                buffer2 = bytes.GetBytes(0x20);
            }
            byte[] dst = new byte[0x31];
            Buffer.BlockCopy(salt, 0, dst, 1, 0x10);
            Buffer.BlockCopy(buffer2, 0, dst, 0x11, 0x20);
            return Convert.ToBase64String(dst);
        }

        public static bool VerifyHashedPassword(string hashedPassword, string password) {
            byte[] buffer4;
            if (hashedPassword == null) {
                return false;
            }
            if (password == null) {
                throw new ArgumentNullException("password");
            }
            byte[] src = Convert.FromBase64String(hashedPassword);
            if ((src.Length != 0x31) || (src[0] != 0)) {
                return false;
            }
            byte[] dst = new byte[0x10];
            Buffer.BlockCopy(src, 1, dst, 0, 0x10);
            byte[] buffer3 = new byte[0x20];
            Buffer.BlockCopy(src, 0x11, buffer3, 0, 0x20);
            using (Rfc2898DeriveBytes bytes = new Rfc2898DeriveBytes(password, dst, 0x3e8)) {
                buffer4 = bytes.GetBytes(0x20);
            }

            return buffer3.SequenceEqual(buffer4);
        }

        public static string DecryptJSBTOA(string base64Encoded) {
            var data = Convert.FromBase64String(base64Encoded);
            var base64Decoded = ASCIIEncoding.ASCII.GetString(data);
            return base64Decoded;
        }

    }
}
