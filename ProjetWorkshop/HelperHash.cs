using System;
using System.Collections.Generic;
using System.Text;
using System.Security.Cryptography;
using System.IO;

namespace ProjetWorkshop
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


        #region OUI

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

        #region RSA

        //public static string CryptRSA(string input)
        //{
        //    string output;

        //    //lets take a new CSP with a new 2048 bit rsa key pair
        //    var csp = new RSACryptoServiceProvider(2048);

        //    //how to get the private key
        //    var privKey = csp.ExportParameters(true);

        //    //and the public key ...
        //    var pubKey = csp.ExportParameters(false);

        //    //converting the public key into a string representation
        //    string pubKeyString;
        //    {
        //        //we need some buffer
        //        var sw = new System.IO.StringWriter();
        //        //we need a serializer
        //        var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
        //        //serialize the key into the stream
        //        xs.Serialize(sw, pubKey);
        //        //get the string from the stream
        //        pubKeyString = sw.ToString();
        //    }

        //    //converting it back
        //    {
        //        //get a stream from the string
        //        var sr = new System.IO.StringReader(pubKeyString);
        //        //we need a deserializer
        //        var xs = new System.Xml.Serialization.XmlSerializer(typeof(RSAParameters));
        //        //get the object back from the stream
        //        pubKey = (RSAParameters)xs.Deserialize(sr);
        //    }

        //    //conversion for the private key is no black magic either ... omitted

        //    //we have a public key ... let's get a new csp and load that key
        //    csp = new RSACryptoServiceProvider();
        //    csp.ImportParameters(pubKey);

        //    //we need some data to encrypt
        //    var plainTextData = "foobar";

        //    //for encryption, always handle bytes...
        //    var bytesPlainTextData = System.Text.Encoding.Unicode.GetBytes(plainTextData);

        //    //apply pkcs#1.5 padding and encrypt our data 
        //    var bytesCypherText = csp.Encrypt(bytesPlainTextData, false);

        //    //we might want a string representation of our cypher text... base64 will do
        //    var cypherText = Convert.ToBase64String(bytesCypherText);


        //    /*
        //     * some transmission / storage / retrieval
        //     * 
        //     * and we want to decrypt our cypherText
        //     */

        //    //first, get our bytes back from the base64 string ...
        //    bytesCypherText = Convert.FromBase64String(cypherText);

        //    //we want to decrypt, therefore we need a csp and load our private key
        //    csp = new RSACryptoServiceProvider();
        //    csp.ImportParameters(privKey);

        //    //decrypt and strip pkcs#1.5 padding
        //    bytesPlainTextData = csp.Decrypt(bytesCypherText, false);

        //    //get our original plainText back...
        //    plainTextData = System.Text.Encoding.Unicode.GetString(bytesPlainTextData);
        //}

        //    return output;
        //}

        #endregion
    }
}
