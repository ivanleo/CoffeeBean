﻿namespace CoffeeBean
{
    /// <summary>
    /// 框架主入口
    /// </summary>
    public static class CMain
    {
        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool HasInit = false;

        /// <summary>
        /// 初始化框架
        /// </summary>
        public static void Init()
        {
            if ( HasInit ) { return; }

            //激活LOG系统
            CLOG.Instance.Start();

            //启动应用程序
            CApp.Instance.Init();

            //开启自动垃圾回收
            CUtilAutoGC.Instance.Begin();

#if DEBUG
            //启动帧率显示
            CFPS.Instance.Begin();
#endif

            //初始化音乐管理器
            CSoundManager.Instance.Init();

            //初始化游戏文件存储
            CLocalizeData.InitGameFolder();

            HasInit = true;
        }
    }
}
