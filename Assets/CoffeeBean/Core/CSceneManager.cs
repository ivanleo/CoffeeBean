﻿/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:31
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CSceneManager.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CSceneManager
    file ext:   cs
    author:     Leo

    purpose:    场景管理器
*********************************************************************/
using System;
using System.Collections;
using System.Reflection;
using UnityEngine;
using UnityEngine.Events;
using UnityEngine.SceneManagement;

namespace CoffeeBean
{
    /// <summary>
    /// 场景加载完毕的委托
    /// </summary>
    public delegate void DelegateSceneLoaded();

    /// <summary>
    /// 场景管理类
    /// </summary>
    public class CSceneManager : CSingletonMono<CSceneManager>
    {
        //异步操作对象
        private AsyncOperation m_AsyncOperator;

        //加载完成回调
        private Action CompleteCallback = null;

        /// <summary>
        /// 苏醒时
        /// </summary>
        private void Awake()
        {
            //注册回调
            SceneManager.sceneLoaded += OnSceneLoaded;

            //暂时用不到的回调
            //SceneManager.sceneUnloaded += OnSceneUnLoaded;
            //SceneManager.activeSceneChanged += OnActiveSceneChanged;
        }

        /// <summary>
        /// 销毁时
        /// </summary>
        protected override void OnDestroy()
        {
            base.OnDestroy();
            //注册回调
            SceneManager.sceneLoaded -= OnSceneLoaded;

            //暂时用不到的回调
            //SceneManager.sceneUnloaded -= OnSceneUnLoaded;
            //SceneManager.activeSceneChanged -= OnActiveSceneChanged;
        }

        /// <summary>
        /// 场景加载完毕回调
        /// </summary>
        /// <param name="TargetScene"></param>
        /// <param name="LoadMode"></param>
        private void OnSceneLoaded ( Scene TargetScene, LoadSceneMode LoadMode )
        {
            CLOG.I ( "Scene {0} Loaded! Load Mode is {1}", TargetScene.name, LoadMode.ToString() );

            if ( CompleteCallback != null )
            {
                CompleteCallback();
                CompleteCallback = null;
            }
        }

        /// <summary>
        /// 场景卸载回调
        /// </summary>
        /// <param name="TargetScene"></param>
        /// <param name="LoadMode"></param>
        private void OnSceneUnLoaded ( Scene TargetScene )
        {
            CLOG.I ( "Scene {0} unLoad!", TargetScene.name );
        }

        /// <summary>
        /// 激活场景改变的回调
        /// </summary>
        private void OnActiveSceneChanged ( Scene OldScene, Scene NewScene )
        {
            CLOG.I ( "Active Scene {0} change to {1}", OldScene.name, NewScene.name );
        }

        /// <summary>
        /// 立刻切换到目标场景
        /// </summary>
        /// <param name="TargetScene">目标场景</param>
        public void ChangeSceneImmediately ( string SceneName, Action LoadCompleteCallback = null )
        {
            try
            {
                CLOG.I ( "ready to load scene {0} immediate", SceneName );
                CompleteCallback = LoadCompleteCallback;
                SceneManager.LoadScene ( SceneName, LoadSceneMode.Single );
            }
            catch ( Exception ex )
            {
                CLOG.E ( ( ex.ToString() ) );
            }
        }

        /// <summary>
        /// 显示一个加载场景来加载场景
        /// 等加载完毕后切换到目标场景
        /// </summary>
        /// <param name="TargetScene">目标场景</param>
        /// <param name="LoadingClass">加载界面类</param>
        /// <param name="Callback">加载完毕的回调</param>
        public void ChangeScene ( string SceneName, Type LoadingClass, Action LoadCompleteCallback = null )
        {
            //记录回调
            CompleteCallback = LoadCompleteCallback;

            try
            {
                //加载并显示场景加载UI
                if ( LoadingClass != null )
                {
                    MethodInfo func = LoadingClass.GetMethod ( "CreateUI", BindingFlags.Static );
                    CAssert.AssertIfNull ( func );
                    func.Invoke ( null, null );
                }
            }
            catch ( Exception ex )
            {
                CLOG.E ( "the LoadingClass has not createUI method EX:{0}", ex.ToString() );
            }

            //开始加载
            StartCoroutine ( LoadScene ( SceneName, LoadingClass == null ) );
        }

        /// <summary>
        /// 异步加载场景
        /// </summary>
        /// <param name="SceneName">场景名</param>
        /// <param name="AutoSwitch">是否在加载完成自动切换到目标场景，如果切换场景使用了加载界面，那么一般需要等加载界面完成动画后再来切换，而没有使用加载界面的，可以自动切换场景</param>
        /// <returns></returns>
        private IEnumerator LoadScene ( string SceneName, bool AutoSwitch )
        {
            CLOG.I ( "ready to load scene {0} asyn", SceneName );
            m_AsyncOperator = SceneManager.LoadSceneAsync ( SceneName );
            m_AsyncOperator.allowSceneActivation = AutoSwitch;
            yield return new WaitUntil ( () => { return m_AsyncOperator.isDone; } );
            m_AsyncOperator = null;
        }

        /// <summary>
        /// 增加一个场景
        /// </summary>
        /// <param name="SceneName"></param>
        public void AddScene ( string SceneName, Action LoadCompleteCallback = null )
        {
            CLOG.I ( "ready to add scene {0}", SceneName );
            SceneManager.LoadScene ( SceneName, LoadSceneMode.Additive );

            //记录回调
            CompleteCallback = LoadCompleteCallback;
        }

        /// <summary>
        /// 得到当前场景
        /// </summary>
        /// <returns></returns>
        public string GetRunningScene()
        {
            return SceneManager.GetActiveScene().name;
        }

        /// <summary>
        /// 得到异步操作对象
        /// 用于获取加载进度
        /// </summary>
        public AsyncOperation AsyncOperator
        {
            get { return m_AsyncOperator; }
        }

        /// <summary>
        /// 得到加载百分比
        /// </summary>
        public float LoadPrecent
        {
            get
            {
                if ( m_AsyncOperator != null )
                {
                    return m_AsyncOperator.progress;
                }
                else
                {
                    return 1f;
                }
            }
        }

#if UNITY_EDITOR
        private void Update()
        {
            if ( m_AsyncOperator != null )
            {
                CLOG.I ( "Load Progress {0}", LoadPrecent );
            }
        }
#endif
    }

}