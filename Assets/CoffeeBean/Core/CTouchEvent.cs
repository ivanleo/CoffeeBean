/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:30
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CTouchEvent.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CTouchEvent
    file ext:   cs
    author:     Leo

    purpose:    UGUI触摸事件模块
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
    public delegate void DelegateTouchEvent ( PointerEventData eventData );

    /// <summary>
    /// UGUI 触摸事件通用组件
    /// 给UGUI中可以被“点击”的组件增加触摸事件
    /// 使用方法
    ///
    ///  Button m_Button = ......
    ///
    ///  设置触摸回调
    ///  m_Button.GetTouchEventModule().OnTouchDown += DeleteteTouchEvent;
    ///  m_Button.GetTouchEventModule().OnTouchUp += DeleteteTouchEvent;
    ///  m_Button.GetTouchEventModule().OnTouchEnter += DeleteteTouchEvent;
    ///  m_Button.GetTouchEventModule().OnTouchExit += DeleteteTouchEvent;
    ///
    ///  清除触摸回调
    ///  m_Button.GetTouchEventModule().OnTouchDown = null;
    ///  m_Button.GetTouchEventModule().OnTouchUp = null;
    ///  m_Button.GetTouchEventModule().OnTouchEnter = null;
    ///  m_Button.GetTouchEventModule().OnTouchExit = null;
    /// </summary>
    public class CTouchEvent : MonoBehaviour, IPointerDownHandler, IPointerUpHandler, IPointerEnterHandler, IPointerExitHandler
    {
        /// <summary>
        /// 按钮按下的事件
        /// </summary>
        public event DelegateTouchEvent OnTouchDown = null;

        /// <summary>
        /// 按钮松开的事件
        /// </summary>
        public event DelegateTouchEvent OnTouchUp = null;

        /// <summary>
        /// 按钮按下滑入的事件
        /// </summary>
        public event DelegateTouchEvent OnTouchEnter = null;

        /// <summary>
        /// 按钮滑出的事件
        /// </summary>
        public event DelegateTouchEvent OnTouchExit = null;

        /// <summary>
        /// 按下事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerDown ( PointerEventData eventData )
        {
            if ( OnTouchDown != null )
            {
                OnTouchDown ( eventData );
            }
        }

        /// <summary>
        /// 松开事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerUp ( PointerEventData eventData )
        {
            if ( OnTouchUp != null )
            {
                OnTouchUp ( eventData );
            }
        }

        /// <summary>
        /// 按下滑入事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerEnter ( PointerEventData eventData )
        {
            if ( OnTouchEnter != null )
            {
                OnTouchEnter ( eventData );
            }
        }

        /// <summary>
        /// 按下滑出事件调度
        /// </summary>
        /// <param name="eventData">事件数据</param>
        public void OnPointerExit ( PointerEventData eventData )
        {
            if ( OnTouchExit != null )
            {
                OnTouchExit ( eventData );
            }
        }
    }


    /// <summary>
    /// 给UGUI增加触摸事件
    /// </summary>
    public static class CUGUITouch
    {
        /// <summary>
        /// 得到一个按钮身上的自定义触摸事件模块
        /// </summary>
        /// <param name="Target">this 扩展 Button按钮</param>
        /// <returns></returns>
        public static CTouchEvent GetTouchEventModule ( this Button Target )
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
        public static CTouchEvent GetTouchEventModule ( this Image Target )
        {
            CTouchEvent ButtonTouchEvent = Target.GetComponent<CTouchEvent>();
            if ( ButtonTouchEvent == null )
            {
                ButtonTouchEvent = Target.gameObject.AddComponent<CTouchEvent>();
            }
            return ButtonTouchEvent;
        }
    }



}