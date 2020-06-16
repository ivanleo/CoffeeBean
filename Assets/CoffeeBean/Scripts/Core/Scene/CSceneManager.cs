/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/14 15:21
	File base:	CSceneManager.cs
	author:		Leo

	purpose:	场景管理类
                提供场景的管理功能

                使用方法
                CSceneManager.LoadScene<场景运行时类>("场景名",加载中回调);
*********************************************************************/

using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.SceneManagement;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 场景管理类
    /// </summary>
    public static class CSceneManager
    {
        /// <summary>
        /// 准备加载的场景绑定类
        /// </summary>
        public static Type ReadyToLoadSceneType ;

        /// <summary>
        /// 是否已经初始化
        /// </summary>
        private static bool m_HasInit = false;

        /// <summary>
        /// 是否正在加载场景
        /// </summary>
        private static bool m_IsLoadingScene = false;

        /// <summary>
        /// 异步加载操作对象
        /// </summary>
        public static AsyncOperation AsyncOperator { get; private set; }

        /// <summary>
        /// 摄像机尺寸
        /// </summary>
        public static float CameraSize { get; set; } = 6.4f;

        /// <summary>
        /// 是否正在加载场景
        /// </summary>
        public static bool IsLoadingScene
        {
            get => m_IsLoadingScene;
            private set
            {
                if ( !m_IsLoadingScene && value )
                {
                    CLOG.I( "scene", "Start Loading Scene" );
                }
                else if ( m_IsLoadingScene && !value )
                {
                    CLOG.I( "scene", "Loading Scene Complete" );
                }

                m_IsLoadingScene = value;
            }
        }

        /// <summary>
        /// 当前场景
        /// </summary>
        public static CSceneBase RunningScene { get; private set; }

        /// <summary>
        /// 初始化
        /// </summary>
        public static void Init()
        {
            if ( m_HasInit )
            {
                return;
            }

            // 提高加载频度，获取更好的加载体验
            Application.backgroundLoadingPriority = ThreadPriority.Low;

            //注册回调
            SceneManager.sceneLoaded += OnSceneLoaded;
            SceneManager.sceneUnloaded += OnSceneUnLoaded;
            SceneManager.activeSceneChanged += OnActiveSceneChanged;

            // 增加主循环
            CApp.Inst.Looper += Update;

            m_HasInit = true;
        }

        /// <summary>
        /// 异步加载场景
        /// 加载完成立刻激活
        /// </summary>
        public static async void LoadScene<T>( string SceneName, Action<float> LoadingCallback ) where T : CSceneBase
        {
            CLOG.I( "scene", $"ready to load scene {SceneName}" );
            // 切换场景时把当前场景设置为不可用
            if ( RunningScene != null )
            {
                RunningScene.IsDirty = true;
            }

            // 设置为正在加载场景
            IsLoadingScene = true;

            // 记录准备加载的场景绑定类
            ReadyToLoadSceneType = typeof( T );

            try
            {
                AsyncOperator = SceneManager.LoadSceneAsync( SceneName );

                // 禁止自动跳转
                AsyncOperator.allowSceneActivation = false;
                while ( !AsyncOperator.isDone )
                {
                    // 当前加载进度
                    float progress = AsyncOperator.progress < 0.9f ? AsyncOperator.progress : 1.0f;

                    // 执行加载回调
                    LoadingCallback?.Invoke( progress );
                    if ( progress >= 1.0f )
                    {
                        AsyncOperator.allowSceneActivation = true;
                    }

                    await new WaitForUpdate();
                }

                IsLoadingScene = false;
            }
            catch ( Exception ex )
            {
                CLOG.E( "scene", ex.ToString() );

                // 遇到异常，切换失败
                if ( RunningScene != null )
                {
                    RunningScene.IsDirty = false;
                }
            }
        }

        /// <summary>
        /// 激活场景改变的回调
        /// </summary>
        private static void OnActiveSceneChanged( Scene OldScene, Scene NewScene )
        {
            CLOG.I( "scene", $"active scene changed:{OldScene.name} ==> {NewScene.name}" );
        }

        /// <summary>
        /// 场景加载完毕回调
        /// </summary>
        /// <param name="TargetScene"></param>
        /// <param name="LoadMode"></param>
        private static void OnSceneLoaded( Scene TargetScene, LoadSceneMode LoadMode )
        {
            var scenename = TargetScene.name;
            CLOG.I( "scene", $"Scene:{scenename} Loaded!" );

            RunningScene = Activator.CreateInstance( ReadyToLoadSceneType ) as CSceneBase;
            RunningScene.BindUnityScene( TargetScene );

            // 初始化场景
            SceneInit( RunningScene );

            // 执行场景进入完毕事件
            RunningScene.AfterEnterScene( TargetScene );
        }

        /// <summary>
        /// 场景卸载回调
        /// </summary>
        /// <param name="TargetScene"></param>
        /// <param name="LoadMode"></param>
        private static void OnSceneUnLoaded( Scene TargetScene )
        {
            CLOG.I( "scene", $"Scene:{TargetScene.name} unLoad!" );

            // 当前场景不为空,且当前场景名相同,则执行退出逻辑
            if ( RunningScene != null && RunningScene.SceneName == TargetScene.name )
            {
                // 执行离开前的处理
                RunningScene.BeforeLeftScene( TargetScene );
                RunningScene = null;
            }
        }

        /// <summary>
        /// 初始化场景
        /// </summary>
        /// <param name="runningScene"></param>
        private static void SceneInit( CSceneBase runningScene )
        {
            runningScene.UICanvas = GameObject.Find( "Canvas" )?.GetComponent<Canvas>();
            runningScene.TopCanvas = GameObject.Find( "TopCanvas" )?.GetComponent<Canvas>();
            runningScene.MainCamera = GameObject.Find( "Main Camera" )?.GetComponent<Camera>();
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        private static void Update()
        {
            if ( RunningScene != null && !RunningScene.IsDirty )
            {
                RunningScene.SceneUpdate();
            }
        }
    }
}