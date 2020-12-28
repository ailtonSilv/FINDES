using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;

public class CryptographyHelper
{
    static string keyString = "E546C8DF278CD5931069B522E695D4F2";
    static List<char> invalidChars = new List<char>() { '\\', '/', '#', '+', '-', '!', '$', '@', '%', '&', '^' };

    public static string Encrypt(string text)
    {
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

    public static string Decrypt(string cipherText)
    {
        var decryptedValue = "";
        var characterException = "";

        try {
            if (cipherText.IndexOf("=__") > 0) {
                characterException = cipherText.Substring(cipherText.IndexOf("=__") + 3);
                cipherText = cipherText.Substring(0, cipherText.LastIndexOf("=")+1);
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
                        foreach (var item in characterException.Split(new char[] {'_'}, StringSplitOptions.RemoveEmptyEntries)) {
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
}
