using System;
using System.Text;

using System.Security.Cryptography;

using System.IO;

namespace UtiltyManagemnt
{
    public class Cryptography
    {

        #region Fields

        private static byte[] _key = { };

        private static readonly byte[] Iv = { 38, 55, 206, 48, 28, 64, 20, 16 };

        private static readonly string stringKey = "!5663a#KN";
        #endregion

        #region Public Methods



        public static string Encrypt(string text)
        {

            try
            {

                _key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                Byte[] byteArray = Encoding.UTF8.GetBytes(text);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateEncryptor(_key, Iv), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);

                cryptoStream.FlushFinalBlock();

                return Convert.ToBase64String(memoryStream.ToArray());

            }

            catch (Exception)
            {

                // Handle Exception Here

            }



            return string.Empty;

        }



        public static string Decrypt(string text)
        {

            try
            {

                _key = Encoding.UTF8.GetBytes(stringKey.Substring(0, 8));

                DESCryptoServiceProvider des = new DESCryptoServiceProvider();

                Byte[] byteArray = Convert.FromBase64String(text);

                MemoryStream memoryStream = new MemoryStream();

                CryptoStream cryptoStream = new CryptoStream(memoryStream,

                des.CreateDecryptor(_key, Iv), CryptoStreamMode.Write);

                cryptoStream.Write(byteArray, 0, byteArray.Length);

                cryptoStream.FlushFinalBlock();

                return Encoding.UTF8.GetString(memoryStream.ToArray());

            }

            catch (Exception)
            {

                // Handle Exception Here

            }



            return string.Empty;

        }



        #endregion

    }

}
