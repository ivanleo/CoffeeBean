using DG.Tweening;
using System;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// UGUI工具类
    /// </summary>
    public static class CUITools
    {
        /// <summary>
        /// 获取一个对象的画布坐标
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static Vector2 GetGameObjectCanvasPos( GameObject target )
        {
            var canvas   = CSceneManager.RunningScene.UICanvas;
            var canvasRT = canvas.transform as RectTransform;
            Vector2 pos;
            var spos = canvas.worldCamera.WorldToScreenPoint( target.transform.position );
            RectTransformUtility.ScreenPointToLocalPointInRectangle( canvasRT, spos, canvas.worldCamera, out pos );
            return pos;
        }

        /// <summary>
        /// 得到屏幕指定点上的UI组件
        /// </summary>
        /// <param name="screenPosition"></param>
        /// <param name="ResultList">结果组件</param>
        /// <returns></returns>
        public static bool GetScreenPosOverUIObjects( Vector2 screenPosition, ref List<RaycastResult> ResultList )
        {
            PointerEventData eventDataCurrentPosition = new PointerEventData( EventSystem.current );
            eventDataCurrentPosition.position = screenPosition;
            EventSystem.current.RaycastAll( eventDataCurrentPosition, ResultList );
            return ResultList.Count > 0;
        }

        /// <summary>
        /// UGUI刷新布局
        /// </summary>
        /// <param name="rect"></param>
        public static void UpdataLayout( RectTransform rect )
        {
            LayoutRebuilder.ForceRebuildLayoutImmediate( rect );
        }
    }
}