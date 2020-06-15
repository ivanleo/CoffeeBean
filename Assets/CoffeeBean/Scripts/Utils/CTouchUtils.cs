/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07 17:01
	File: 	    CTouchUtils.cs
	Author:		Leo

	Purpose:	触摸工具类

*********************************************************************/

using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 触摸工具
    /// </summary>
    public static class CTouchUtils
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
}