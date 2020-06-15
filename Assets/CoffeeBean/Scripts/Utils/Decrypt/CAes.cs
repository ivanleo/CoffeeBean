using System;
using System.Collections.Generic;
using System.Linq;
using System.Security.Cryptography;
using System.Text;
using System.Threading.Tasks;

namespace CoffeeBean
{
    public class CAes
    {
        /// <summary>
        /// AES加密
        /// </summary>
        /// <param name="source">原始字节流</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static byte[] AESEncrypt( byte[] source, string password, string iv )
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            byte[] keyBytes = Encoding.UTF8.GetBytes( password );
            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = Encoding.UTF8.GetBytes( iv );
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform   = rijndaelCipher.CreateEncryptor();
            byte[]           cipherBytes = transform.TransformFinalBlock( source, 0, source.Length );

            return cipherBytes;
        }

        /// <summary>
        /// AES解密
        /// </summary>
        /// <param name="encryptedData">原始字节流</param>
        /// <param name="password">加密的密码</param>
        /// <param name="iv">密钥</param>
        /// <returns></returns>
        public static byte[] AESDecrypt( byte[] encryptedData, string password, string iv )
        {
            RijndaelManaged rijndaelCipher = new RijndaelManaged();
            rijndaelCipher.Mode = CipherMode.CBC;
            rijndaelCipher.Padding = PaddingMode.PKCS7;
            rijndaelCipher.KeySize = 128;
            rijndaelCipher.BlockSize = 128;

            byte[] keyBytes = Encoding.UTF8.GetBytes( password );
            rijndaelCipher.Key = keyBytes;

            byte[] ivBytes = Encoding.UTF8.GetBytes( iv );
            rijndaelCipher.IV = ivBytes;

            ICryptoTransform transform = rijndaelCipher.CreateDecryptor();
            byte[]           plainText = transform.TransformFinalBlock( encryptedData, 0, encryptedData.Length );

            return plainText;
        }
    }
}