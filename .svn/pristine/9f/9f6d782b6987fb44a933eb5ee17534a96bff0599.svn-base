using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace CoffeeBean
{
    public static class CTime
    {
        /// <summary>
        /// 时间戳转时间字符串
        /// </summary>
        /// <param name="timeStamp">时间戳</param>
        /// <param name="formatString">时间格式串
        /// 常见的有 yyyy/MM/dd HH:mm:ss:ffff
        ///          yyyy/MM/dd
        ///          HH:mm:ss
        /// </param>
        /// <returns></returns>
        public static string TimeStampToTimeString ( int timeStamp, string formatString )
        {
            //开始时间
            DateTime startTime = TimeZone.CurrentTimeZone.ToLocalTime ( new DateTime ( 1970, 1, 1 ) ); // 当地时区
            DateTime dt = startTime.AddSeconds ( timeStamp );
            return dt.ToString ( formatString );
        }

        /// <summary>
        /// 得到当前时间戳
        /// </summary>
        /// <param name="needMilliSeconds">是否获取基于豪秒的时间戳</param>
        /// <returns></returns>
        public static long GetNowTimeStamp ( bool needMilliSeconds = false )
        {
            TimeSpan ts = DateTime.UtcNow - new DateTime ( 1970, 1, 1, 0, 0, 0, 0 );
            long ret;
            if ( needMilliSeconds )
            {
                ret = Convert.ToInt64 ( ts.TotalMilliseconds );
            }
            else
            {
                ret = Convert.ToInt64 ( ts.TotalSeconds );
            }
            return ret;
        }

        /// <summary>
        /// 得到秒的字符串表述
        /// 如调用函数
        /// 传递参数    返回结果
        ///  60（S）    1分钟
        ///  90（S）    1分30秒
        ///  3600（S）  1小时
        ///  5430（S）  1小时30分30秒
        /// </summary>
        /// <param name="Second"></param>
        /// <returns></returns>
        public static string GetSecondText ( int Second )
        {
            if ( Second < 60 )
            {
                return Second.ToString() + "秒";
            }

            int day = Second / 86400;
            int hour = ( Second % 86400 ) / 3600;
            int min = ( Second % 3600 ) / 60;
            int sec = Second % 60;

            string daystr = day > 0 ? day.ToString() + "天" : string.Empty;
            string hourstr = hour > 0 ? hour.ToString() + "小时" : string.Empty;
            string minstr = min.ToString() + "分" + ( sec == 0 ? "钟" : string.Empty );
            string secstr = sec > 0 ? sec.ToString() + "秒" : string.Empty;

            return daystr + hourstr + minstr + secstr;
        }
    }
}
