/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/7 17:14:35
   File: 	   CMD5.cs
   Author:     Leo

   Purpose:    MD5加密类
*********************************************************************/

using System;
using System.IO;
using System.Security.Cryptography;
using System.Text;

namespace CoffeeBean
{
    /// <summary>
    /// MD5加密类
    /// </summary>
    public static class CMD5
    {
        /// <summary>
        /// MD5 加密字符串
        ///
        /// 使用方法 string xx = "123456"; xx.MD5();
        ///
        /// "123456".MD5();
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <returns>加密后的字符串</returns>
        public static string MD5( this string source )
        {
            return MD5Encrypt( source );
        }

        /// <summary>
        /// MD5 加密
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static string MD5Encrypt( string source )
        {
            byte[] result = Encoding.Default.GetBytes( source.Trim() );
            MD5    md5    = new MD5CryptoServiceProvider();
            byte[] output = md5.ComputeHash( result );
            return BitConverter.ToString( output ).Replace( "-", "" );
        }

        /// <summary>
        /// 文件MD5加密 Hash
        /// </summary>
        /// <param name="filePath">文件路径</param>
        /// <returns></returns>
        public static string MD5FileHash( string filePath )
        {
            try
            {
                FileStream fs = new FileStream(filePath, FileMode.Open);
                int len = (int)fs.Length;
                byte[] data = new byte[len];
                fs.Read( data, 0, len );
                fs.Close();

                MD5 md5 = new MD5CryptoServiceProvider();
                byte[] output = md5.ComputeHash(data);
                return BitConverter.ToString( output ).Replace( "-", "" );
            }
            catch ( FileNotFoundException e )
            {
                Console.WriteLine( e.Message );
                return "";
            }
        }
    }
}