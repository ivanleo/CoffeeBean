/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/7 16:24:38
   File: 	   Class1.cs
   Author:     Leo

   Purpose:    整形数字扩展
*********************************************************************/

namespace CoffeeBean
{
    /// <summary>
    /// 整形数字扩展
    /// </summary>
    public static class CExpandInt
    {
        /// <summary>
        /// 缩写数字
        /// 如 10000 缩写为 1万
        /// 如 12345 缩写为 1.234万
        /// </summary>
        public static string ToNumAbridge( this int source )
        {
            var str = source.ToString();

            if ( str.Length > 8 )
            {
                var point = str.Substring(str.Length - 8, 1);
                if ( point == "0" )
                {
                    return $"{str.Substring( 0, str.Length - 8 )}亿";
                }
                else
                {
                    return $"{str.Substring( 0, str.Length - 8 )}.{point}亿";
                }
            }

            if ( str.Length > 4 )
            {
                var point = str.Substring(str.Length - 4, 1);
                if ( point == "0" )
                {
                    return $"{str.Substring( 0, str.Length - 4 )}万";
                }
                else
                {
                    return $"{str.Substring( 0, str.Length - 4 )}.{point}万";
                }
            }

            return str;
        }
    }
}