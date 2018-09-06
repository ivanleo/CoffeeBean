/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:32
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CApp.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CApp
    file ext:   cs
    author:     Leo

    purpose:    应用程序功能类
*********************************************************************/
#if UNITY_EDITOR
    using UnityEditor;
#endif
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 应用程序状态
    /// </summary>
    public delegate void DelegateAppState();

    /// <summary>
    /// 应用程序类
    /// </summary>
    public class CApp : CSingletonMono<CApp>
    {
        /// <summary>
        /// 切换到前台事件
        /// 移动端/编辑器下 以
        /// </summary>
        public event DelegateAppState EventAppSwitchIn = null;

        /// <summary>
        /// 切换到后台事件
        /// </summary>
        public event DelegateAppState EventAppSwitchOut = null;

        /// <summary>
        /// 退出应用程序事件
        /// </summary>
        public event DelegateAppState EventAppQuit = null;

        /// <summary>
        /// 应用程序是否暂停
        /// </summary>
        public bool IsApplecationPause { get; set; }

        /// <summary>
        /// 应用程序是否获得焦点
        /// </summary>
        public bool IsApplecationFocus { get; set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public void Init()
        {

        }

        /// <summary>
        /// 应用退出
        /// </summary>
        private void OnApplicationQuit()
        {
            CLOG.I ( "Application Quit" );
            if ( EventAppQuit != null )
            {
                EventAppQuit();
            }
        }

        /// <summary>
        /// 应用程序暂停状态变化
        /// </summary>
        /// <param name="pause"></param>
        private void OnApplicationPause ( bool pause )
        {
            CLOG.I ( "Application Pause state {0}", pause );
            IsApplecationPause = pause;
        }

        /// <summary>
        /// 应用程序焦点变化
        /// </summary>
        /// <param name="focus"></param>
        private void OnApplicationFocus ( bool focus )
        {
            CLOG.I ( "Application Focus state {0}", focus );
            IsApplecationFocus = focus;

            if ( focus )
            {
                CheckSwitchIn();
            }
            else
            {
                CheckSwitchOut();
            }
        }

        /// <summary>
        /// 检查是否切换到前台
        /// 移动端以是否丢失焦点作为游戏切换的依据
        /// </summary>
        private void CheckSwitchIn()
        {
            if ( IsApplecationFocus == true && EventAppSwitchIn != null )
            {
                EventAppSwitchIn();
            }
        }

        /// <summary>
        /// 检查是否切换到后台
        /// 移动端以是否丢失焦点作为游戏切换的依据
        /// </summary>
        private void CheckSwitchOut()
        {
            if ( IsApplecationFocus == false && EventAppSwitchOut != null )
            {
                EventAppSwitchOut();
            }
        }

        /// <summary>
        /// 退出
        /// </summary>
        public void Exit()
        {
            CLOG.I ( "End Game" );

#if UNITY_EDITOR
            EditorApplication.isPlaying = false;
            return;
#else
            Application.Quit();
            return;
#endif

        }
    }
}

