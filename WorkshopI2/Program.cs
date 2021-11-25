using System;

namespace ProjetWorkshop
{
    class Program
    {
        static void Main(string[] args)
        {
            HelperConfig helper = new HelperConfig();
            string[] test = HelperHash.AleatoireChiffrement();
            string message = "First, you should verify whether the input string is valid. Then, you need to define an instance of the";
            string final = EncryptData(message, test);
            string finalDecrypt = DecryptData(final, test);

        }

        

        private static string EncryptData(string input, string[] chiffrements)
        {
            string output = input;
            foreach(string chiffrement in chiffrements)
            {
                switch(chiffrement)
                {
                    case "AES":
                        output = HelperHash.CryptAES(output);
                        break;
                    case "MD5":
                        output = HelperHash.CryptMD5(output);
                        break;
                    case "DES":
                        output = HelperHash.CryptDES(output);
                        break;
                }
            }

            return output;
        }

        private static string DecryptData(string input, string[] chiffrements)
        {
            string output = input;

            for(int i = chiffrements.Length-1; (i < chiffrements.Length && i >=0 ); i--)
            {
                switch(chiffrements[i])
                {
                    case "AES":
                        output = HelperHash.DecryptAES(output);
                        break;
                    case "MD5":
                        output = HelperHash.DeCryptMD5(output);
                        break;
                    case "DES":
                        output = HelperHash.DeCryptDES(output);
                        break;
                }
            }

            return output;
        }




    }
}
