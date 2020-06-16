/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 22:59
	File base:	CLOG.cs
	author:		Leo

	purpose:	LOG工具类
                提供LOG输出和记录本地文件的功能

                未来可添加远程LOG收集
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Diagnostics;
using System.IO;
using System.Text.RegularExpressions;
using System.Threading;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// LOG类
    /// </summary>
    public static class CLOG
    {
        // 是否正在运行
        private static bool m_IsRunning = false;

        // 锁
        private static object m_Locker;

        // 待写消息队列
        private static Queue<string> m_LogQueue;

        // 消息线程
        private static Thread m_LogThread;

        // 写入流
        private static StreamWriter m_LogWriter;

        /// <summary>
        /// LOG系统是否正在工作
        /// </summary>
        public static bool IsRunning => m_IsRunning;

        /// <summary>
        /// 对应安卓上的 LogError 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void E( string logStr )
        {
            UnityEngine.Debug.LogError( GetLogString( "", logStr, "ERROR" ) );
        }

        /// <summary>
        /// 对应安卓上的 LogError 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void E( string tag, string logStr )
        {
            UnityEngine.Debug.LogError( GetLogString( tag, logStr, "ERROR" ) );
        }

        /// <summary>
        /// 对应安卓上的 LogInfo 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void I( string logStr )
        {
            UnityEngine.Debug.Log( GetLogString( "", logStr, "INFO" ) );
        }

        // ---------------------- LOG 输出函数 ----------------------
        /// <summary>
        /// 对应安卓上的 LogInfo 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void I( string tag, string logStr )
        {
            UnityEngine.Debug.Log( GetLogString( tag, logStr, "INFO" ) );
        }

        /// <summary>
        /// 开始Debug的Logger功能
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void Init()
        {
            if ( m_IsRunning )
            {
                return;
            }

            m_LogQueue = new Queue<string>();
            m_Locker = new object();

            // 文件时间
            DateTime now    = DateTime.Now;
            string   month  = now.Month.ToString().PadLeft ( 2, '0' );
            string   day    = now.Day.ToString().PadLeft ( 2, '0' );
            string   hour   = now.Hour.ToString().PadLeft ( 2, '0' );
            string   minute = now.Minute.ToString().PadLeft ( 2, '0' );
            string   second = now.Second.ToString().PadLeft ( 2, '0' );

            string logName = $"Log_{now.Year}_{month}_{day}_{hour}_{minute}_{second}.txt";
            string logPath = $"{CApp.Inst.Log_Path}/{logName}";

            // 文件存在则删除
            if ( File.Exists( logPath ) )
            {
                File.Delete( logPath );
            }

            UnityEngine.Debug.Log( $"log system actived! write to {logPath}" );

            // 写文件对象
            m_LogWriter = new StreamWriter( logPath );
            m_LogWriter.AutoFlush = true;
            m_IsRunning = true;

            // 写线程
            m_LogThread = new Thread( Process );

            // 设置为后台线程，随主线程关闭而关闭
            m_LogThread.IsBackground = true;
            m_LogThread.Start();

            // 添加Log处理
            Application.logMessageReceived += OnLogHandler;
        }

        /// <summary>
        /// 停止记log
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void Stop()
        {
            lock ( m_Locker )
            {
                m_IsRunning = false;
                m_LogQueue.Clear();
                m_LogWriter.Close();
            }
        }

        /// <summary>
        /// 对应安卓上的 LogWarning 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void W( string logStr )
        {
            UnityEngine.Debug.LogWarning( GetLogString( "", logStr, "WARN" ) );
        }

        /// <summary>
        /// 对应安卓上的 LogWarning 输出
        /// </summary>
        [Conditional( "NEED_LOG" )]
        public static void W( string tag, string logStr )
        {
            UnityEngine.Debug.LogWarning( GetLogString( tag, logStr, "WARN" ) );
        }

        /// <summary>
        /// 得到Log字符串
        /// </summary>
        /// <param name="tag">标签</param>
        /// <param name="str">LOG字符串</param>
        /// <param name="typeString">类型串</param>
        /// <returns></returns>
        private static string GetLogString( string tag, string str, string typeString )
        {
            DateTime now        = DateTime.Now;
            string   millsecond = now.Millisecond.ToString().PadLeft ( 3, '0' );
            typeString = typeString.PadRight( 5, ' ' );
            if ( tag.Length > 0 )
            {
                return $"[{now}.{millsecond} | {typeString}] <{tag}> {str}";
            }
            else
            {
                return $"[{now}.{millsecond}] {str}";
            }
        }

        /// <summary>
        /// Log处理
        /// </summary>
        /// <param name="log">log内容</param>
        /// <param name="stackTrace">堆栈信息</param>
        /// <param name="type">log类型</param>
        private static void OnLogHandler( string log, string stackTrace, LogType type )
        {
            if ( !m_IsRunning )
            {
                return;
            }

            lock ( m_Locker )
            {
                switch ( type )
                {
                case LogType.Assert:
                case LogType.Error:
                case LogType.Exception:
                    PushLog( log );
                    PushLog( "---------------[ Stack ]---------------" );
                    PushLog( stackTrace );
                    break;

                case LogType.Log:
                case LogType.Warning:
                    PushLog( log );
                    break;
                }
            }
        }

        /// <summary>
        /// 从队列中取一个log文字来打印
        /// </summary>
        /// <returns>队列中的文字</returns>
        private static string PopLog()
        {
            if ( m_LogQueue.Count == 0 )
            {
                return "";
            }

            lock ( m_Locker )
            {
                return m_LogQueue.Dequeue();
            }
        }

        /// <summary>
        /// 打印log
        /// </summary>
        private static void Process()
        {
            while ( m_IsRunning )
            {
                string logStr;

                while ( ( logStr = PopLog() ).Length > 0 )
                {
                    m_LogWriter.WriteLine( logStr );
                }

                Thread.Sleep( 1 );
            }
        }

        /// <summary>
        /// 将要打印的log压入队列
        /// </summary>
        /// <param name="msg"></param>
        [Conditional( "NEED_LOG" )]
        private static void PushLog( string msg )
        {
            m_LogQueue.Enqueue( msg );
        }
    }
}