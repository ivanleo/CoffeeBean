using System;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeBean
{
    /// <summary>
    /// MD5加密封装
    /// </summary>
    public static class CExpandString
    {
        /// <summary>
        /// 加密字符串
        /// 使用方法
        ///    string xx = "123456";
        ///    xx.MD5();
        ///
        ///    "123456".MD5();
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5( this string source )
        {
            byte[] result = Encoding.Default.GetBytes( source.Trim() );
            MD5 md5 = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash( result );
            return BitConverter.ToString( output ).Replace( "-", "" );
        }
    }

}