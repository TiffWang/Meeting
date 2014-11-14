using System;
using System.Text;
using System.Collections.Generic;
using System.Security.Cryptography;
using System.IO;

namespace Infrastructure
{
    /// <summary>
    /// this class is used to do TripleDES Encrypt/Decrypt
    /// </summary>
    public static class Security
    {
        private const int KEY_SIZE          = 192;
        private const string DEFAULT_KEY    = "&hd22*DW@132";
        private const string DEFAULT_IV     = "BB(3';.321M";

        /// <summary>
        /// convert the string to byte array
        /// </summary>
        /// <param name="str">string</param>
        /// <param name="length">the byte length</param>
        /// <returns>the byte array</returns>
        private static byte[] ConvertToByteArray(string str, int length)
        {
            var buf = Encoding.UTF8.GetBytes(str);
            var ret = new byte[length / 8];

            int index;
            for (index = 0; index < buf.Length; index++)
            {
                ret[index] = buf[index];
            }

            for (; index < ret.Length; index++)
            {
                ret[index] = 0x40;
            }

            return ret;
        }


        /// <summary>
        /// Use the 3DES to encrypt text with default encode(UTF-16)
        /// </summary>
        /// <param name="buffer">text buffer</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Encrypted</returns>
        public static string TripleDESEncrypt(byte[] buffer, byte[] key, byte[] iv)
        {
            StringBuilder ret = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                ret = new StringBuilder();
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                des.KeySize = KEY_SIZE;
                CryptoStream cs = new CryptoStream(ms, des.CreateEncryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(buffer, 0, buffer.Length);
                cs.FlushFinalBlock();
                foreach (byte b in ms.ToArray()) ret.AppendFormat("{0:X2}", b);
            }
            catch
            {
                ret = null;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
            return ret == null ? null : ret.ToString();
        }


        /// <summary>
        /// Use the 3DES to encrypt text with default encode(UTF-16)
        /// </summary>
        /// <param name="buffer">text buffer</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Encrypted</returns>
        public static string TripleDESEncrypt(byte[] buffer, string key, string iv)
        {
            return TripleDESEncrypt(buffer
                , ConvertToByteArray(key, KEY_SIZE)
                , ConvertToByteArray(iv, KEY_SIZE)
                );
        }

        /// <summary>
        /// Use the 3DES to encrypt text with default encode(UTF-16)
        /// </summary>
        /// <param name="buffer">text buffer</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Encrypted</returns>
        public static string TripleDESEncrypt(byte[] buffer, byte[] key, string iv)
        {
            return TripleDESEncrypt(buffer
                , key
                , ConvertToByteArray(iv, KEY_SIZE)
                );
        }


        /// <summary>
        /// Use the 3DES to encrypt text with default encode(UTF-16)
        /// </summary>
        /// <param name="buffer">text buffer</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Encrypted</returns>
        public static string TripleDESEncrypt(byte[] buffer, string key, byte[] iv)
        {
            return TripleDESEncrypt(buffer
                , ConvertToByteArray(key, KEY_SIZE)
                , iv
                );
        }



        /// <summary>
        /// Decrypt encrypted with default encode(UTF-16) in 3DES
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Byte array of decryption</returns>
        public static byte[] TripleDESDecrypt(string encrypted, byte[] key, byte[] iv)
        {
            byte[] ret = null;
            MemoryStream ms = new MemoryStream();
            try
            {
                TripleDESCryptoServiceProvider des = new TripleDESCryptoServiceProvider();
                byte[] result = new byte[encrypted.Length / 2];
                for (int x = 0; x < encrypted.Length / 2; x++)
                {
                    int i = (System.Convert.ToInt32(encrypted.Substring(x * 2, 2), 16));
                    result[x] = (byte)i;
                }
                des.KeySize = KEY_SIZE;
                CryptoStream cs = new CryptoStream(ms, des.CreateDecryptor(key, iv), CryptoStreamMode.Write);
                cs.Write(result, 0, result.Length);
                cs.FlushFinalBlock();
                ret = ms.ToArray();
            }
            catch
            {
                ret = null;
            }
            finally
            {
                ms.Close();
                ms.Dispose();
            }
            return ret;
        }


        /// <summary>
        /// Decrypt encrypted with default encode(UTF-16) in 3DES
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Byte array of decryption</returns>
        public static byte[] TripleDESDecrypt(string encrypted, string key, string iv)
        {
            return TripleDESDecrypt(encrypted
                , ConvertToByteArray(key, KEY_SIZE)
                , ConvertToByteArray(iv, KEY_SIZE)
                );
        }


        /// <summary>
        /// Decrypt encrypted with default encode(UTF-16) in 3DES
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Byte array of decryption</returns>
        public static byte[] TripleDESDecrypt(string encrypted, byte[] key, string iv)
        {
            return TripleDESDecrypt(encrypted
                , key
                , ConvertToByteArray(iv, KEY_SIZE)
                );
        }


        /// <summary>
        /// Decrypt encrypted with default encode(UTF-16) in 3DES
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <param name="key">Key</param>
        /// <param name="iv">IV</param>
        /// <returns>Byte array of decryption</returns>
        public static byte[] TripleDESDecrypt(string encrypted, string key, byte[] iv)
        {
            return TripleDESDecrypt(encrypted
                , ConvertToByteArray(key, KEY_SIZE)
                , iv
                );
        }


        /// <summary>
        /// Encrypt text in MD5
        /// </summary>
        /// <param name="text">Text</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Encrypted</returns>
        public static string MD5Encrypt(string text, Encoding encoding)
        {
            MD5CryptoServiceProvider md5 = new MD5CryptoServiceProvider();
            byte[] result = encoding.GetBytes(text);
            result = md5.ComputeHash(result);
            StringBuilder ret = new StringBuilder();
            foreach (byte b in result) ret.AppendFormat("{0:X2}", b);
            return ret.ToString();
        }

        /// <summary>
        /// Encrypt text with UTF-16 encoding in MD5
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Encrypted</returns>
        public static string MD5Encrypt(string text)
        {
            return MD5Encrypt(text, Encoding.Unicode);
        }

        /// <summary>
        /// Decrypt encrypted with default key and encode(UTF-16) in 3DES
        /// </summary>
        /// <param name="buffer">Encrypted buffer</param>
        /// <returns>Decryption</returns>
        public static string DefaultTripleDESEncrypt(byte[] buffer)
        {
            return TripleDESEncrypt(buffer
                , ConvertToByteArray(DEFAULT_KEY, KEY_SIZE)
                , ConvertToByteArray(DEFAULT_IV, KEY_SIZE)
                );
        }

        /// <summary>
        /// Decrypt encrypted by default key and encode(UTF-16)
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <returns>Decryption</returns>
        public static byte [] DefaultTripleDESDecrypt(string encrypted)
        {
            return TripleDESDecrypt(encrypted
                , ConvertToByteArray(DEFAULT_KEY, KEY_SIZE)
                , ConvertToByteArray(DEFAULT_IV, KEY_SIZE)
                );
        }

        /// <summary>
        /// Use the 3DES to encrypt text with default key
        /// </summary>
        /// <param name="text">Text content</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Encrypted</returns>
        public static string DefaultEncrypt(string text, Encoding encoding)
        {
            byte[] buf = encoding.GetBytes(text);
            return TripleDESEncrypt( buf
                , ConvertToByteArray( DEFAULT_KEY, KEY_SIZE)
                , ConvertToByteArray( DEFAULT_IV, KEY_SIZE)
                );
        }

        /// <summary>
        /// Use the 3DES to encrypt text with default key and encode(UTF-16)
        /// </summary>
        /// <param name="text">Text content</param>
        /// <returns>Encrypted</returns>
        public static string DefaultEncrypt(string text)
        {
            return DefaultEncrypt(text, Encoding.Unicode);
        }


        /// <summary>
        /// Decrypt encrypted by default key
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <param name="encoding">Encoding</param>
        /// <returns>Decryption</returns>
        public static string DefaultDecrypt(string encrypted, Encoding encoding)
        {
            byte[] buf = TripleDESDecrypt( encrypted
                , ConvertToByteArray( DEFAULT_KEY, KEY_SIZE)
                , ConvertToByteArray( DEFAULT_IV, KEY_SIZE)
                );
            return buf == null ? null: encoding.GetString(buf).Trim('\0');
        }

        /// <summary>
        /// Decrypt encrypted by default key and encode(UTF-16)
        /// </summary>
        /// <param name="encrypted">Encrypted</param>
        /// <returns>Decryption</returns>
        public static string DefaultDecrypt(string encrypted)
        {
            return DefaultDecrypt(encrypted, Encoding.Unicode);
        }

        private static Random __base_random__ = new Random();
        /// <summary>
        /// Automatically generate codes by conditions.
        /// </summary>
        /// <param name="count">Max total of codes that generate for.</param>
        /// <param name="hasCapitalLetter">Is there any capital letters exists in codes?</param>
        /// <param name="hasLowercase">Is there any lowercases exists in codes?</param>
        /// <param name="hasDigit">Is there any digits exists in codes?</param>
        /// <param name="hasInterpunction">Is there any interpunctions exists in codes?</param>
        /// <returns>Codes</returns>
        public static string GenerateCode(int count, bool hasCapitalLetter, bool hasLowercase, bool hasDigit, bool hasInterpunction)
        {
            string result = String.Empty;
            if (count < 1) return result;

            string capitalLetter = @"ABCDEFGHIJKLMNOPQRSTUVWXYZ";
            string lowercase = @"abcdefghijklmnopqrstuvwxyz";
            string digit = @"0123456789";
            string interpunction = @"~!@#$%^&*()_+=-{}|\][:""';<>?/.,";

            List<bool> matchs = new List<bool>();
            List<string> condis = new List<string>();
            if (hasCapitalLetter)
            {
                condis.Add(capitalLetter);
                matchs.Add(false);
            }
            if (hasLowercase)
            {
                condis.Add(lowercase);
                matchs.Add(false);
            }
            if (hasDigit)
            {
                condis.Add(digit);
                matchs.Add(false);
            }
            if (hasInterpunction)
            {
                condis.Add(interpunction);
                matchs.Add(false);
            }

            int conditionCount = condis.Count;
            int unmatchedCount = conditionCount;
            if (conditionCount == 0) return result;

            while (count > 0)
            {
                int baseSeed = __base_random__.Next(0, conditionCount);
                if (!matchs[baseSeed])
                {
                    matchs[baseSeed] = true;
                    unmatchedCount--;
                }
                else if (count <= unmatchedCount)
                {
                    condis.RemoveAt(baseSeed);
                    matchs.RemoveAt(baseSeed);
                    conditionCount = condis.Count;
                    continue;
                }
                result += condis[baseSeed][__base_random__.Next(0, condis[baseSeed].Length)];
                count--;
            }
            return result;
        }


        /// <summary>
        /// Automatically generate codes that the codes contains at least 1 capital Letter, 1 lowercase, 1 digit and 1 interpunction.
        /// </summary>
        /// <param name="count">Max total of codes that generate for.</param>
        /// <returns>Codes</returns>
        public static string GenerateCode(int count)
        {
            return GenerateCode(count, true, true, true, true);
        }

        /// <summary>
        /// Automatically generate codes that the codes contains at least 1 capital Letter, 1 lowercase and 1 digit
        /// </summary>
        /// <param name="count">Max total of codes that generate for.</param>
        /// <param name="hasInterpunction">Is there any interpunctions exists in codes?</param>
        /// <returns>Codes</returns>
        public static string GenerateCode(int count, bool hasInterpunction)
        {
            return GenerateCode(count, true, true, true, hasInterpunction);
        }
    }
}
