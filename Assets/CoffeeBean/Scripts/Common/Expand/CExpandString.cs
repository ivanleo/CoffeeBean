/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/08 10:50
	File:       CExpandString.cs
	Author:		Leo

	Purpose:	扩展字符串
                提供更多成员方法
*********************************************************************/

using System;
using System.Security.Cryptography;
using System.Text;
using System.IO;
using System.Collections.Generic;

namespace CoffeeBean
{
    /// <summary>
    /// 字符串扩展
    /// </summary>
    public static class CExpandString
    {
        /// <summary>
        /// 获取一维字符串数组的整形 如 字符串 str = "123|324|541|432334" 执行 str.ConvertToIntAry() 返回值 = int[]{123,324,541,432334}
        /// 本方法常用于配置表中数组数据的提取
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="sp">分隔符，默认 |</param>
        /// <returns></returns>
        public static int[] ConvertToIntAry( this string source, char sp = '|' )
        {
            var   strs  = source.Split( sp );
            int[] datas = new int[strs.Length];
            for ( int i = 0; i < strs.Length; i++ )
            {
                datas[i] = int.Parse( strs[i] );
            }

            return datas;
        }

        /// <summary>
        /// 获取二维字符串数组里的整形值 如 字符串 str = "123|324|541|432334#333|23|43" 执行
        /// GetIntArrayData_TwoDimens(str) 返回值 = int[][]{{123,324,541,432334},{333,23,43}}
        /// </summary>
        /// <param name="source">源字符串</param>
        /// <param name="sp1">第二维分隔符，默认#</param>
        /// <param name="sp2">第一维分隔符，默认|</param>
        /// <returns></returns>
        public static int[][] ConvertToIntAryAry( this string source, char sp1 = '#', char sp2 = '|' )
        {
            var     strs1 = source.Split( sp1 );
            int[][] datas = new int[strs1.Length][];

            for ( int i = 0; i < strs1.Length; i++ )
            {
                var strs2 = strs1[i].Split( sp2 );
                var data  = new int[strs2.Length];
                for ( int j = 0; j < strs2.Length; j++ )
                {
                    data[j] = int.Parse( strs2[j] );
                }

                datas[i] = data;
            }

            return datas;
        }

        /// <summary>
        /// 返回一个字符串是有意义的 非空 有内容即为有意义
        /// </summary>
        /// <param name="source"></param>
        /// <returns>有意义返回 true 否则 false</returns>
        public static bool IsNotNullAndEmpty( this string source )
        {
            return !String.IsNullOrEmpty( source );
        }

        /// <summary>
        /// 返回一个字符串是否为空的
        /// </summary>
        /// <param name="source"></param>
        /// <returns>为空或者长度为0返回 true 否则 false</returns>
        public static bool IsNullOrEmpty( this string source )
        {
            return String.IsNullOrEmpty( source );
        }

        /// <summary>
        /// 执行字符串或运算
        /// <para></para>
        /// <para>string a = null;</para>
        /// <para>string b = "aa</para>
        /// <para>string c = a.Or(b);</para>
        /// <para>c="aa"</para>
        /// </summary>
        /// <param name="source"></param>
        /// <param name="target"></param>
        /// <returns></returns>
        public static string Or( this string source, string target )
        {
            return source == null ? target : source;
        }

        /// <summary>
        /// 重复一个字符N次
        /// </summary>
        /// <param name="source"></param>
        /// <param name="count"></param>
        /// <returns></returns>
        public static string Repeat( this char source, int count )
        {
            StringBuilder sb = new StringBuilder();
            sb.Append( source, count );
            return sb.ToString();
        }
    }
}