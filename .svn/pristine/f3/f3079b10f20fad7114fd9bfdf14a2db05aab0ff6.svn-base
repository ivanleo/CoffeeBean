/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:41
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Debuger\// CLOG.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Debuger
    file base:  CLOG
    file ext:   cs
    author:     Leo

    purpose: LOG工具类
*********************************************************************/

using System;
using System.Collections.Generic;
using System.IO;
using System.Threading;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// LOG输出类
    /// </summary>
    public class CLOG : CSingleton<CLOG>
    {
        // 输出路径
        private string m_OutputDir;

        // 待写消息队列
        private Queue<string> m_LogQueue = new Queue<string>();

        // 消息线程
        private Thread m_LogThread;

        // 锁
        private object m_Locker = new object();

        // 是否正在运行
        private bool m_IsRunning = false;

        // 写入流
        private StreamWriter m_LogWriter = null;


        /// <summary>
        /// 开始Debug的Logger功能
        /// </summary>
        public void Start()
        {
#if UNITY_EDITOR
            //调试模式输出到资源目录同级的Log文件夹
            m_OutputDir = Application.dataPath + "/../Log";
#elif UNITY_ANDROID
            //安卓下输出到持久化目录
            m_OutputDir = Application.persistentDataPath;
#endif

            if ( m_IsRunning )
            {
                return;
            }

            //文件时间
            DateTime now = DateTime.Now;
            string month = now.Month.ToString().PadLeft ( 2, '0' );
            string day = now.Day.ToString().PadLeft ( 2, '0' );
            string hour = now.Hour.ToString().PadLeft ( 2, '0' );
            string minute = now.Minute.ToString().PadLeft ( 2, '0' );
            string second = now.Second.ToString().PadLeft ( 2, '0' );

            string logName = string.Format ( "Log_{0}_{1}_{2}_{3}_{4}_{5}.txt", now.Year, month, day, hour, minute, second );
            string logPath = string.Format ( "{0}/{1}", m_OutputDir, logName );
            if ( File.Exists ( logPath ) )
            {
                File.Delete ( logPath );
            }

            string logDir = Path.GetDirectoryName ( logPath );
            if ( !Directory.Exists ( logDir ) )
            {
                Directory.CreateDirectory ( logDir );
            }

            //写文件对象
            m_LogWriter = new StreamWriter ( logPath );
            m_LogWriter.AutoFlush = true;

            m_IsRunning = true;

            //写线程
            m_LogThread = new Thread ( Process );
            m_LogThread.IsBackground = true;
            m_LogThread.Start();

            //添加Log处理
            Application.logMessageReceivedThreaded += OnLogHandler;

            Debug.Log ( "log system actived!" );
        }

        /// <summary>
        /// Log处理
        /// </summary>
        /// <param name="log">log内容</param>
        /// <param name="stackTrace">堆栈信息</param>
        /// <param name="type">log类型</param>
        private void OnLogHandler ( string log, string stackTrace, LogType type )
        {
            lock ( m_Locker )
            {
                switch ( type )
                {
                    case LogType.Assert:
                    case LogType.Error:
                    case LogType.Exception:
                        Instance.PushLog ( log );
                        Instance.PushLog ( "---------------[ Stack ]---------------" );
                        Instance.PushLog ( stackTrace );
                        break;
                    case LogType.Log:
                    case LogType.Warning:
                        Instance.PushLog ( log );
                        break;
                }
            }
        }

        /// <summary>
        /// 停止记log
        /// </summary>
        public void Stop()
        {
            lock ( m_Locker )
            {
                m_IsRunning = false;
                m_LogQueue.Clear();
                m_LogWriter.Close();
            }
        }

        /// <summary>
        /// LOG系统是否正在工作
        /// </summary>
        public bool IsRunning
        {
            get { return m_IsRunning; }
        }

        /// <summary>
        /// 打印log
        /// </summary>
        private void Process()
        {
            while ( m_IsRunning )
            {
                string logStr;
                while ( ( logStr = PopLog() ).Length > 0 )
                {
                    m_LogWriter.WriteLine ( logStr );
                }

                Thread.Sleep ( 1 );
            }
        }

        // 将要打印的log压入队列
        private void PushLog ( string msg )
        {
            m_LogQueue.Enqueue ( msg );
        }

        /// <summary>
        /// 从队列中取一个log文字来打印
        /// </summary>
        /// <returns>队列中的文字</returns>
        private string PopLog()
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
        /// 对应安卓上的 LogVerbose 输出
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void V ( string format, params object[] args )
        {
            string str = GetLogString ( string.Format ( format, args ), "Verbose" );
            Debug.Log ( str );
        }

        /// <summary>
        /// 对应安卓上的 LogError 输出
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void E ( string format, params object[] args )
        {
            string str = GetLogString ( string.Format ( format, args ), "Error" );
            Debug.LogError ( str );
        }


        /// <summary>
        /// 对应安卓上的 LogInfo 输出
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void I ( string format, params object[] args )
        {
            string str = GetLogString ( string.Format ( format, args ), "Info" );
            Debug.Log ( str );

        }

        /// <summary>
        /// 对应安卓上的 LogWarning 输出
        /// </summary>
        /// <param name="format">格式字符串</param>
        /// <param name="args">格式参数</param>
        public static void W ( string format, params object[] args )
        {
            string str = GetLogString ( string.Format ( format, args ), "Warning" );
            Debug.LogWarning ( str );
        }


        /// <summary>
        /// LOG输出一个空行
        /// </summary>
        public static void BlankRow()
        {
            Instance.PushLog ( " " );
        }

        /// <summary>
        /// LOG输出一个空行
        /// </summary>
        public static void BR()
        {
            BlankRow();
        }

        /// <summary>
        /// 得到Log字符串
        /// </summary>
        /// <param name="str"></param>
        /// <param name="typeString"></param>
        /// <returns></returns>
        private static string GetLogString ( string str, string typeString )
        {
            DateTime now = System.DateTime.Now;
            string month = now.Month.ToString().PadLeft ( 2, '0' );
            string day = now.Day.ToString().PadLeft ( 2, '0' );
            string hour = now.Hour.ToString().PadLeft ( 2, '0' );
            string minute = now.Minute.ToString().PadLeft ( 2, '0' );
            string second = now.Second.ToString().PadLeft ( 2, '0' );
            string millsecond = now.Millisecond.ToString().PadLeft ( 3, '0' );
            return string.Format ( "[{0}-{1}-{2} {3}:{4}:{5}.{6} | {7}] {8}", now.Year, month, day, hour, minute, second, millsecond, typeString.PadRight ( 8 ), str );
        }
    }
}
