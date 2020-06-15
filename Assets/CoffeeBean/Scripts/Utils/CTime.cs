/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/6 19:09:35
   File: 	   CTime.cs
   Author:     Leo

   Purpose:    时间相关工具类
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeBean
{
    /// <summary>
    /// 时间相关工具类
    /// </summary>
    public static class CTime
    {
        /// <summary>
        /// 将dateTime格式转换为Unix时间戳(秒）
        /// </summary>
        /// <param name="dateTime"></param>
        /// <returns></returns>
        public static int DateTimeToUnixTimeStamp( DateTime dateTime )
        {
            return (int)( dateTime - TimeZone.CurrentTimeZone.ToLocalTime( new DateTime( 1970, 1, 1 ) ) ).TotalSeconds;
        }

        /// <summary>
        /// 得到当前时间时间戳
        /// </summary>
        /// <param name="ms">是否获取基于豪秒的时间戳</param>
        /// <returns></returns>
        public static long GetNowTimeStamp( bool ms = false )
        {
            TimeSpan ts = DateTime.Now - new DateTime ( 1970, 1, 1, 0, 0, 0, 0 );
            return ms ? Convert.ToInt64( ts.TotalMilliseconds ) : Convert.ToInt64( ts.TotalSeconds );
        }

        /// <summary>
        /// 得到秒的字符串表述 如调用函数 传递参数 返回结果 60（S） 1分钟 90（S） 1分30秒 3600（S） 1小时 5430（S） 1小时30分30秒
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static string GetSecondCNText( int Second )
        {
            if ( Second < 0 )
            {
                Second = 0;
            }

            if ( Second < 60 )
            {
                return Second.ToString() + "秒";
            }

            int day  = Second / 86400;
            int hour = ( Second % 86400 ) / 3600;
            int min  = ( Second % 3600 ) / 60;
            int sec  = Second % 60;

            string daystr  = day > 0 ? day.ToString() + "天" : string.Empty;
            string hourstr = hour > 0 ? hour.ToString() + "小时" : string.Empty;
            string minstr  = min.ToString() + "分" + ( sec == 0 ? "钟" : string.Empty );
            string secstr  = sec > 0 ? sec.ToString() + "秒" : string.Empty;

            return daystr + hourstr + minstr + secstr;
        }

        /// <summary>
        /// 获取秒的时间显示 如 GetSecondText(5430) =&gt; 01:30:30 常用于倒计时
        /// </summary>
        /// <param name="sec"></param>
        public static string GetSecondText( int Second )
        {
            if ( Second < 0 )
            {
                Second = 0;
            }

            int hour = Second / 3600;
            int min  = ( Second - hour * 3600 ) / 60;
            int sec  = Second % 60;
            var hourstr = hour.ToString().PadLeft ( 2, '0' );
            var minstr  = min.ToString().PadLeft ( 2, '0' );
            var secstr  = sec.ToString().PadLeft ( 2, '0' );
            return $"{hourstr}:{minstr}:{secstr}";
        }

        /// <summary>
        /// 以时间作为版本号
        /// </summary>
        /// <returns></returns>
        public static ulong GetTimeVersion()
        {
            DateTime now    = DateTime.Now;
            string   month  = now.Month.ToString().PadLeft ( 2, '0' );
            string   day    = now.Day.ToString().PadLeft ( 2, '0' );
            string   hour   = now.Hour.ToString().PadLeft ( 2, '0' );
            string   minute = now.Minute.ToString().PadLeft ( 2, '0' );
            string   second = now.Second.ToString().PadLeft ( 2, '0' );

            string timeversion = string.Format ( "{0}{1}{2}{3}{4}{5}", now.Year, month, day, hour, minute, second );
            return ulong.Parse( timeversion );
        }

        /// <summary>
        /// 得到当前标准时间时间戳
        /// </summary>
        /// <param name="ms">是否获取基于豪秒的时间戳</param>
        /// <returns></returns>
        public static long GetUTCTimeStamp( bool ms = false )
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime ( 1970, 1, 1, 0, 0, 0, 0 );
            return ms ? Convert.ToInt64( ts.TotalMilliseconds ) : Convert.ToInt64( ts.TotalSeconds );
        }

        /// <summary>
        /// 时间戳(秒)转时间字符串
        /// </summary>
        /// <param name="timeStamp">时间戳(秒)</param>
        /// <param name="formatString">时间格式串 常见的有 yyyy/MM/dd HH:mm:ss:ffff yyyy/MM/dd HH:mm:ss</param>
        /// <returns></returns>
        public static string TimeStampToTimeString( int timeStamp, string formatString = "yyyy/MM/dd HH:mm:ss" )
        {
            //开始时间
            DateTime startTime = new DateTime ( 1970, 1, 1 );
            DateTime dt        = startTime.AddSeconds ( timeStamp );
            return dt.ToString( formatString );
        }

        /// <summary>
        /// 将Unix时间戳(秒）转换为dateTime格式
        /// </summary>
        /// <param name="time"></param>
        /// <returns></returns>
        public static DateTime UnixTimeStampToDateTime( int time )
        {
            if ( time < 0 )
            { throw new ArgumentOutOfRangeException( "time is out of range" ); }

            return TimeZone.CurrentTimeZone.ToLocalTime( new DateTime( 1970, 1, 1 ) ).AddSeconds( time );
        }
    }
}