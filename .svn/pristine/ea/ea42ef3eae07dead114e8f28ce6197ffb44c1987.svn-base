/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:27
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CUIBase.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CUIBase
    file ext:   cs
    author:     Leo

    purpose:    UI基类
*********************************************************************/
using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// UI基类接口
    /// </summary>
    public interface IUIBase
    {
        /// <summary>
        /// 是否纳入层级管理器
        /// </summary>
        bool IsInLayoutManager { get; set; }

        /// <summary>
        /// 创建时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        void AnimOnCreate(  );

        /// <summary>
        /// 隐藏时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        void AnimOnHide(  );

        /// <summary>
        /// 显示时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        void AnimOnShow(  );

        /// <summary>
        /// 销毁时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        void AnimOnDestroy ( TweenCallback callback = null );

        /// <summary>
        /// 显示UI
        /// </summary>
        void ShowUI();

        /// <summary>
        /// 隐藏UI
        /// </summary>
        void HideUI();

    }

    /// <summary>
    /// UI基类
    /// </summary>
    public abstract class CUIBase<T> : MonoBehaviour, IUIBase where T : CUIBase<T>
    {
        /// <summary>
        /// UI单例引用
        /// </summary>
        private static T m_UIInstance = null;

        /// <summary>
        /// UI非单例引用“们”
        /// </summary>
        private static List<T> m_UIInstances = new List<T>();

        /// <summary>
        /// UI的单例引用，非单例获取则为空
        /// </summary>
        public static T UIInstance { get { return m_UIInstance; } private set { m_UIInstance = value; } }

        /// <summary>
        /// UI非单例引用“们”
        /// </summary>
        public static List<T> UIInstances { get { return m_UIInstances; } private set { m_UIInstances = value; } }

        /// <summary>
        /// 矩形变换组件
        /// </summary>
        public RectTransform rectTransform { get { return transform as RectTransform; } }

        /// <summary>
        /// 是否纳入层级管理器
        /// </summary>
        public bool IsInLayoutManager { get; set; }

        /// <summary>
        /// 被记录的UI信息
        /// </summary>
        public CUIInfo UIInfo { get; private set; }

        /// <summary>
        /// 创建时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        public virtual void AnimOnCreate()
        {
            //放大显示
            transform.localScale = Vector3.zero;
            transform.DOScale ( Vector3.one, 0.5f );
        }

        /// <summary>
        /// 隐藏时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        public virtual void AnimOnHide()
        {
            //放大,淡化消失
            transform.localScale = Vector3.one;
            transform.DOScale ( new Vector3 ( 1.3f, 1.3f, 1.3f ), 0.5f );
            transform.FadeOutUINode ( 0.5f );
        }

        /// <summary>
        /// 显示时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        public virtual void AnimOnShow()
        {
            //缩小,淡化进入
            transform.localScale = new Vector3 ( 1.3f, 1.3f, 1.3f );
            transform.DOScale ( Vector3.one, 0.5f );
            transform.FadeInUINode ( 0.5f );
        }

        /// <summary>
        /// 销毁时的动画
        /// 子类可以继承后修改他实现自己的动画效果
        /// </summary>
        public virtual void AnimOnDestroy ( TweenCallback callback = null )
        {
            //放大显示
            transform.localScale = Vector3.one;
            transform.DOScale ( Vector3.zero, 0.5f ).OnComplete ( callback );
        }

        /// <summary>
        /// 呈现UI
        /// </summary>
        public void ShowUI()
        {
            if ( UIInfo.IsAnimationUI )
            {
                AnimOnShow();
            }
        }

        /// <summary>
        /// 隐藏UI
        /// </summary>
        public void HideUI()
        {
            if ( UIInfo.IsAnimationUI )
            {
                AnimOnHide();
            }
        }


        /// <summary>
        /// 创建层次UI
        /// </summary>
        /// <returns></returns>
        public static T CreateLayoutUI()
        {
            T UI = CreateUI();

            //添加倒层级管理器
            CUILayoutManager.PushUI ( UI );
            return UI;
        }

        /// <summary>
        /// 创建UI的界面显示
        /// </summary>
        public static T CreateUI()
        {
            Type Tp = typeof ( T );

            // 遍历特性
            foreach ( var attr in Tp.GetCustomAttributes ( false ) )
            {
                //UI预制体特性
                if ( attr.GetType() == typeof ( CUIInfo ) )
                {
                    return CreateUI ( attr as CUIInfo );
                }
            }

            CLOG.E ( "the ui {0} has no CUIInfo attr", typeof ( T ).ToString() );
            return null;
        }

        /// <summary>
        /// 创建UI
        /// </summary>
        /// <param name="PrefabInfo"></param>
        /// <returns></returns>
        private static T CreateUI ( CUIInfo UIInfo )
        {
            if ( UIInfo.IsSigleton ) //单例UI
            {
                if ( m_UIInstance == null ) //实例不存在
                {
                    m_UIInstance = InstantiateUIAndReturnComponent ( UIInfo );
                }

                return m_UIInstance;
            }
            else  //非单例UI
            {
                T TempComp = InstantiateUIAndReturnComponent ( UIInfo );
                UIInstances.Add ( TempComp );
                return TempComp;
            }
        }

        /// <summary>
        /// 创建UI实例并返回其上的UI组件
        ///
        /// </summary>
        /// <param name="PrefabName">预制体名</param>
        /// <param name="NeedAnimation">是否需要动画</param>
        /// <returns></returns>
        private static T InstantiateUIAndReturnComponent ( CUIInfo UIInfo )
        {
            //找当前场景中的画布
            GameObject _Canvas = GameObject.Find ( "Canvas" );

            if ( _Canvas == null )
            {
                CLOG.E ( "now scene has not canvas" );
                return null;
            }

            //创建UI预制体实例
            //非单例UI自动缓存
            //单例UI不缓存
            GameObject CreatedUI = CResourcesManager.CreatePrefab ( UIInfo.PrefabName, !UIInfo.IsSigleton ); ;

            if ( CreatedUI == null )
            {
                CLOG.E ( "the UI {0} create failed", UIInfo.PrefabName );
                return null;
            }

            //创建UI界面;
            RectTransform RT = CreatedUI.GetComponent<RectTransform>();

            //设置父节点
            CreatedUI.transform.SetParent ( _Canvas.transform, false );
            RT.localScale = Vector3.one;

            //向UI上添加自身组件
            T Ins = GetComponentSafe ( CreatedUI );

            //记录UI信息
            Ins.UIInfo = UIInfo;
            Ins.IsInLayoutManager = false;

            //执行创建动画
            if ( UIInfo.IsAnimationUI )
            {
                Ins.AnimOnCreate();
            }

            //返回UI实例
            return Ins;
        }

        /// <summary>
        /// 安全的得到组件
        /// </summary>
        /// <param name="Target">目标游戏对象</param>
        /// <returns></returns>
        private static T GetComponentSafe ( GameObject Target )
        {
            T TempCom = Target.GetComponent<T>();
            if ( TempCom == null )
            {
                TempCom = Target.AddComponent<T>();
            }

            return TempCom;
        }

        /// <summary>
        /// 干掉UI界面
        /// 单例界面干掉单例
        /// 非单例界面干掉所有非单例
        /// </summary>
        public static bool DestroyUI()
        {
            if ( UIInstance != null )
            {
                //单例UI，安全销毁
                return SafeDelete ( UIInstance );
            }

            if ( m_UIInstances.Count > 0 )
            {
                //非单例UI的正确删除情况

                //1. 若非单例UI中，存在2个被纳入层级管理的情况，则不允许删除，因为系统无法同时删除2个层级UI
                //2. 若非单例UI中，存在1个被纳入层级管理，且处于栈顶时，可以删除
                //3. 若非单例UI中，存在1个被纳入层级管理，且不处于栈顶时，不可以删除
                //4. 若非单例UI中，所有UI都未被纳入层级管理，则正常删除所有UI

                int layerUICount = 0;
                int layerUIIndex = -1;

                //检查是否存在2个被纳入UI层级管理器的UI
                for ( int i = 0; i < m_UIInstances.Count; i++ )
                {
                    if ( m_UIInstances[i].IsInLayoutManager )
                    {
                        layerUICount++;
                        layerUIIndex = i;

                        //有超过1个的UI被纳入层级管理器的UI
                        //无法直接删除
                        if ( layerUICount > 1 )
                        {
                            CLOG.E ( "the un singleton ui {0} has {1} ui in layoutmanager ,so can not destroy!", typeof ( T ).Name, layerUICount );
                            return false;
                        }
                    }
                }

                if ( layerUICount <= 1 )
                {
                    //存在层级UI的情况
                    if ( layerUIIndex != -1 )
                    {
                        //先尝试删除层级UI
                        if ( SafeDelete ( m_UIInstances[layerUIIndex] ) )
                        {
                            //可以删除的话，先删除掉
                            m_UIInstances.RemoveAt ( layerUIIndex );
                        }
                        else
                        {
                            CLOG.E ( "the un singleton ui {0} is not the top ui in layoutmanager ,so can not destroy!", typeof ( T ).Name );
                            return false;
                        }
                    }

                    //清空剩余非单例子UI
                    for ( int i = m_UIInstances.Count - 1; i >= 0; i-- )
                    {
                        SafeDelete ( m_UIInstances[i] );
                    }

                    m_UIInstances.Clear();
                }
            }
            return true;
        }

        /// <summary>
        /// 销毁指定界面
        /// </summary>
        /// <param name="target">目标</param>
        public static bool DestroyUI ( T target )
        {
            //单例UI直接销毁
            if ( target == UIInstance )
            {
                return SafeDelete ( target );
            }

            //销毁非单例UI
            T findObj = UIInstances.Find ( ( T obj ) =>
            {
                return obj == target;
            } );

            //没找倒
            if ( findObj == null )
            {
                CLOG.E ( "the ui {0} != UIInstance && not in UIInstances!", target.name );
                return false;
            }

            //成功删除就移除记录
            if ( SafeDelete ( target ) )
            {
                m_UIInstances.Remove ( target );
            }
            else
            {
                return false;
            }

            return true;
        }


        /// <summary>
        /// 销毁
        /// </summary>
        public virtual void OnDestroy()
        {
            if ( m_UIInstance != null )
            {
                m_UIInstance = null;
            }

            if ( m_UIInstances.Contains ( ( T ) this ) )
            {
                m_UIInstances.Remove ( ( T ) this );
            }
        }

        /// <summary>
        /// 安全删除UI
        /// </summary>
        private static bool SafeDelete ( CUIBase<T> UI )
        {
            if ( !CUILayoutManager.PopUI ( UI ) )
            {
                return false;
            }

            //删除UI
            if ( UI.UIInfo.IsAnimationUI )
            {
                //支持动画的话就播完动画再销毁
                UI.AnimOnDestroy ( () =>
                {
                    GameObject.Destroy ( UI.gameObject );
                } );
            }
            else
            {
                //不支持动画直接销毁
                GameObject.Destroy ( UI.gameObject );
            }

            return true;

        }

    }
}


