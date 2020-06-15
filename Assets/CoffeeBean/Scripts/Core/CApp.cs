/********************************************************************
   All Right Reserved By Leo
   Created:	    2020/6/5 20:35:15
   File: 	    CApp.cs
   Author:      Leo

   Purpose:	    APP类
                封装了一些常用事件
*********************************************************************/

using System;

using UnityEngine;
using System.IO;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CoffeeBean
{
    /// <summary>
    /// 设备网络状态
    /// </summary>
    public enum ENetState
    {
        /// <summary>
        /// 无网络
        /// </summary>
        NONE,

        /// <summary>
        /// Wifi连接中
        /// </summary>
        WIFI,

        /// <summary>
        /// 移动网络3G/4G连接中
        /// </summary>
        MOBILE_3G_4G,
    }

    /// <summary>
    /// 应用程序类
    /// </summary>
    public class CApp : CSingletonMono<CApp>
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private bool hasInit = false;

        /// <summary>
        /// http下载目录
        /// </summary>
        public static string Download_Path => Application.persistentDataPath + "/../download/";

        ///// <summary>
        ///// Log目录
        ///// </summary>
#if UNITY_EDITOR
        public static string Log_Path => Application.dataPath + "/../Log/";
#else
        public static string Log_Path => Application.persistentDataPath + "/../Log/";
#endif

        /// <summary>
        /// 图片下载目录
        /// </summary>
        public static string Texture_Path => Application.persistentDataPath + "/image/";

        /// <summary>
        /// 应用程序是否获得焦点
        /// </summary>
        public bool IsApplecationFocus { get; private set; }

        /// <summary>
        /// 应用程序是否暂停
        /// </summary>
        public bool IsApplecationPause { get; private set; }

        /// <summary>
        /// 多点触摸状态
        /// </summary>
        public bool MutiTouchEnabled { get => Input.multiTouchEnabled; set => Input.multiTouchEnabled = value; }

        /// <summary>
        /// 当前网络环境，wifi，3G/4G ,还是网络未连接
        /// </summary>
        public ENetState NetState
        {
            get
            {
                switch ( Application.internetReachability )
                {
                case NetworkReachability.ReachableViaLocalAreaNetwork:
                    return ENetState.WIFI;

                case NetworkReachability.ReachableViaCarrierDataNetwork:
                    return ENetState.MOBILE_3G_4G;

                default:
                    return ENetState.NONE;
                }
            }
        }

        /// <summary>
        /// 目标帧率
        /// </summary>
        public int TargetFrameRate { get => Application.targetFrameRate; set => Application.targetFrameRate = value; }

        /// <summary>
        /// 退出应用程序事件
        /// </summary>
        public event Action EventAppQuit = null;

        /// <summary>
        /// 切换到前台事件
        /// </summary>
        public event Action EventAppSwitchIn = null;

        /// <summary>
        /// 切换到后台事件
        /// </summary>
        public event Action EventAppSwitchOut = null;

        /// <summary>
        /// 主循环
        /// </summary>
        public event Action Looper;

        /// <summary>
        /// 退出
        /// </summary>
        public void Exit()
        {
#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
#else
            Application.Quit();
#endif
        }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {
            if ( hasInit )
            {
                return;
            }

            TargetFrameRate = 60;
            MutiTouchEnabled = false;

            // 初始化常用文件夹
            InitDirectory();

            hasInit = true;
        }

        /// <summary>
        /// 初始化常用目录
        /// 检查设备下是否具备上述目录
        /// 没有则创建
        /// </summary>
        public void InitDirectory()
        {
            var DI = new DirectoryInfo ( Log_Path );

            if ( !DI.Exists )
            {
                DI.Create();
            }

            DI = new DirectoryInfo( Download_Path );

            if ( !DI.Exists )
            {
                DI.Create();
            }

            DI = new DirectoryInfo( Texture_Path );

            if ( !DI.Exists )
            {
                DI.Create();
            }

            hasInit = true;
        }

        /// <summary>
        /// 应用程序焦点变化
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus( bool focus )
        {
            CLOG.I( "app", $"Application Focus state {focus}" );
            IsApplecationFocus = focus;

            if ( focus )
            {
                // 检查是否切换到前台
                // 移动端以是否丢失焦点作为游戏切换的依据
                EventAppSwitchIn?.Invoke();
            }
            else
            {
                // 检查是否切换到后台
                // 移动端以是否丢失焦点作为游戏切换的依据
                EventAppSwitchOut?.Invoke();
            }
        }

        /// <summary>
        /// 应用程序暂停状态变化
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause( bool pause )
        {
            CLOG.I( "app", $"Application Pause state {pause}" );
            IsApplecationPause = pause;
        }

        /// <summary>
        /// 应用退出
        /// </summary>
        private void OnApplicationQuit()
        {
            CLOG.I( "app", "Application Quit" );
            EventAppQuit?.Invoke();
        }

        /// <summary>
        /// APP每帧更新
        /// </summary>
        private void Update()
        {
            Looper?.Invoke();
        }
    }
}