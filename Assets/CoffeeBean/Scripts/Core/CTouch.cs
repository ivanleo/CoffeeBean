/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07 17:01
	File: 	    CTouch.cs
	Author:		Leo

	Purpose:	UGUI触摸事件模块
                给UGUI中可以被“点击”的组件增加触摸事件
                使用方法

                Button m_Button = ......

                设置触摸回调
                m_Button.GetTouchEventModule().OnTouchDown += DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchUp += DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchEnter += DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchExit += DeleteteTouchEvent;

                清除触摸回调
                m_Button.GetTouchEventModule().OnTouchDown -= DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchUp -= DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchEnter -= DeleteteTouchEvent;
                m_Button.GetTouchEventModule().OnTouchExit -= DeleteteTouchEvent;

*********************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 触摸事件
    /// </summary>
    /// <param name="eventData">事件参数</param>
    public delegate void DGTouchEvent( PointerEventData eventData );

    /// <summary>
    /// 触摸工具
    /// </summary>
    public static class CTouchTools
    {
        /// <summary>
        /// 是否点到UI上
        /// </summary>
        /// <returns></returns>
        public static bool IsTouchedUI()
        {
            bool touchedUI = false;

            //判断是否点击UI
            if ( Input.GetMouseButtonDown( 0 ) || ( Input.touchCount > 0 && Input.GetTouch( 0 ).phase == TouchPhase.Began ) )
            {
                //TODO 移动端
                if ( Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer )
                {
                    if ( Input.touchCount > 0 && EventSystem.current != null && EventSystem.current.IsPointerOverGameObject( Input.GetTouch( 0 ).fingerId ) )
                    {
                        touchedUI = true;
                    }
                }
                //TODO PC端
                else if ( EventSystem.current != null && EventSystem.current.IsPointerOverGameObject() )
                {
                    touchedUI = true;
                }
            }

            return touchedUI;
        }
    }

    /// <summary>
    /// UI事件模块
    /// </summary>
    public static class CUIEventModule
    {
        /// <summary>
        /// 得到一个按钮身上的自定义触摸事件模块
        /// </summary>
        /// <param name="Target">this 扩展 Button按钮</param>
        /// <returns></returns>
        public static CTouchEvent GetTouchEventModule( this Button Target )
        {
            CTouchEvent ButtonTouchEvent = Target.GetComponent<CTouchEvent>();

            if ( ButtonTouchEvent == null )
            {
                ButtonTouchEvent = Target.gameObject.AddComponent<CTouchEvent>();
            }

            return ButtonTouchEvent;
        }

        /// <summary>
        /// 得到一个按钮身上的自定义触摸事件模块
        /// </summary>
        /// <param name="Target">this 扩展 Button按钮</param>
        /// <returns></returns>
        public static CTouchEvent GetTouchEventModule( this Image Target )
        {
            CTouchEvent ButtonTouchEvent = Target.GetComponent<CTouchEvent>();

            if ( ButtonTouchEvent == null )
            {
                ButtonTouchEvent = Target.gameObject.AddComponent<CTouchEvent>();
            }

            return ButtonTouchEvent;
        }
    }

    /// <summary>
    /// UGUI 触摸事件通用组件
    /// </summary>
    public class CTouchEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 按钮按下的事件
        /// </summary>
        public event DGTouchEvent OnTouchDown = null;

        /// <summary>
        /// 按钮按下滑入的事件
        /// </summary>
        public event DGTouchEvent OnTouchEnter = null;

        /// <summary>
        /// 按钮滑出的事件
        /// </summary>
        public event DGTouchEvent OnTouchExit = null;

        /// <summary>
        /// 按钮松开的事件
        /// </summary>
        public event DGTouchEvent OnTouchUp = null;

        /// <summary>
        /// 按下事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerDown( PointerEventData eventData )
        {
            OnTouchDown?.Invoke( eventData );
        }

        /// <summary>
        /// 按下滑入事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerEnter( PointerEventData eventData )
        {
            OnTouchEnter?.Invoke( eventData );
        }

        /// <summary>
        /// 按下滑出事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerExit( PointerEventData eventData )
        {
            OnTouchExit?.Invoke( eventData );
        }

        /// <summary>
        /// 松开事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerUp( PointerEventData eventData )
        {
            OnTouchUp?.Invoke( eventData );
        }
    }
}