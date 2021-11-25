using System;
using System.Collections.Generic;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace WorkshopI2
{
    class HelperHash
    {

        public static List<string> MethodesChiffrement = new List<string>
        {
            "AES",
            "MD5",
            "DES"
        };

        public static string[] AleatoireChiffrement()
        {
            Random rand = new Random();
            string[] chiffrements = new string[3];

            for (int i = 0; i < 3; i++)
            {
                chiffrements[i] = HelperHash.MethodesChiffrement[rand.Next(0, HelperHash.MethodesChiffrement.Count)];
            }

            return chiffrements;
        }

        public static string EncryptData(string input, string[] chiffrements)
        {
            string output = input;
            foreach (string chiffrement in chiffrements)
            {
                switch (chiffrement)
                {
                    case "AES":
                        output = CryptAES(output);
                        break;
                    case "MD5":
                        output = CryptMD5(output);
                        break;
                    case "DES":
                        output = CryptDES(output);
                        break;
                }
            }

            return output;
        }

        public static string DecryptData(string input, string[] chiffrements)
        {
            string output = input;

            for (int i = chiffrements.Length - 1; (i < chiffrements.Length && i >= 0); i--)
            {
                switch (chiffrements[i])
                {
                    case "AES":
                        output = DecryptAES(output);
                        break;
                    case "MD5":
                        output = DeCryptMD5(output);
                        break;
                    case "DES":
                        output = DeCryptDES(output);
                        break;
                }
            }

            return output;
        }

        #region MD5

        static string keyMD5 { get; set; } = "A!9HHhi%XjjYY4YP2@Nob009X";


        public static string CryptMD5(string input)
        {
            string output;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(keyMD5));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateEncryptor())
                    {
                        byte[] textBytes = UTF8Encoding.UTF8.GetBytes(input);
                        byte[] bytes = transform.TransformFinalBlock(textBytes, 0, textBytes.Length);
                        output = Convert.ToBase64String(bytes, 0, bytes.Length);
                    }
                }
            }

            return output;
        }

        public static string DeCryptMD5(string input)
        {
            string output;
            using (var md5 = new MD5CryptoServiceProvider())
            {
                using (var tdes = new TripleDESCryptoServiceProvider())
                {
                    tdes.Key = md5.ComputeHash(UTF8Encoding.UTF8.GetBytes(keyMD5));
                    tdes.Mode = CipherMode.ECB;
                    tdes.Padding = PaddingMode.PKCS7;

                    using (var transform = tdes.CreateDecryptor())
                    {
                        byte[] cipherBytes = Convert.FromBase64String(input);
                        byte[] bytes = transform.TransformFinalBlock(cipherBytes, 0, cipherBytes.Length);
                        output = UTF8Encoding.UTF8.GetString(bytes);
                    }
                }
            }
            return output;
        }

        #endregion

        #region AES

        static string PrivKeyAES { get; set; } = "12345678";
        static string PubKeyAES { get; set; } = "87654321";
        public static string CryptAES(string input)
        {
            string output;

            byte[] secretkeyByte = System.Text.Encoding.UTF8.GetBytes(PrivKeyAES);
            byte[] publickeybyte = System.Text.Encoding.UTF8.GetBytes(PubKeyAES);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = System.Text.Encoding.UTF8.GetBytes(input);
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateEncryptor(publickeybyte, secretkeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                output = Convert.ToBase64String(ms.ToArray());
            }

            return output;
        }

        public static string DecryptAES(string input)
        {
            string output;

            byte[] privatekeyByte = System.Text.Encoding.UTF8.GetBytes(PrivKeyAES);
            byte[] publickeybyte = System.Text.Encoding.UTF8.GetBytes(PubKeyAES);
            MemoryStream ms = null;
            CryptoStream cs = null;
            byte[] inputbyteArray = new byte[input.Replace(" ", "+").Length];
            inputbyteArray = Convert.FromBase64String(input.Replace(" ", "+"));
            using (DESCryptoServiceProvider des = new DESCryptoServiceProvider())
            {
                ms = new MemoryStream();
                cs = new CryptoStream(ms, des.CreateDecryptor(publickeybyte, privatekeyByte), CryptoStreamMode.Write);
                cs.Write(inputbyteArray, 0, inputbyteArray.Length);
                cs.FlushFinalBlock();
                Encoding encoding = Encoding.UTF8;
                output = encoding.GetString(ms.ToArray());
            }

            return output;
        }

        #endregion


        #region DES

        static string PrivKeyDES { get; set; } = "12345678";
        static string PubKeyDES { get; set; } = "87654321";

        public static string CryptDES(string input)
        {
            string output;

            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException
                       ("The string which needs to be encrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream();
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateEncryptor(Encoding.UTF8.GetBytes(PrivKeyDES), Encoding.UTF8.GetBytes(PubKeyDES)), CryptoStreamMode.Write);
            StreamWriter writer = new StreamWriter(cryptoStream);
            writer.Write(input);
            writer.Flush();
            cryptoStream.FlushFinalBlock();
            writer.Flush();
            output = Convert.ToBase64String(memoryStream.GetBuffer(), 0, (int)memoryStream.Length);

            return output;

        }

        public static string DeCryptDES(string input)
        {
            string output;

            if (String.IsNullOrEmpty(input))
            {
                throw new ArgumentNullException
                   ("The string which needs to be decrypted can not be null.");
            }
            DESCryptoServiceProvider cryptoProvider = new DESCryptoServiceProvider();
            MemoryStream memoryStream = new MemoryStream
                    (Convert.FromBase64String(input));
            CryptoStream cryptoStream = new CryptoStream(memoryStream,
                cryptoProvider.CreateDecryptor(Encoding.UTF8.GetBytes(PrivKeyDES), Encoding.UTF8.GetBytes(PubKeyDES)), CryptoStreamMode.Read);
            StreamReader reader = new StreamReader(cryptoStream);
            output = reader.ReadToEnd();

            return output;
        }
        #endregion
    }
}
