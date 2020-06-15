/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 23:33
	File base:	CFPS.cs
	author:		Leo

	purpose:	显示FPS信息
*********************************************************************/

using UnityEngine;
using CodeStage.AdvancedFPSCounter;

namespace CoffeeBean
{
    /// <summary>
    /// FPS信息
    /// </summary>
    public static class CFPS
    {
        /// <summary>
        /// fps 组件
        /// </summary>
        private static AFPSCounter fps = null;

        /// <summary>
        /// 隐藏FPS显示
        /// </summary>
        public static void Hide()
        {
            if ( fps != null )
            {
                fps.OperationMode = OperationMode.Disabled;
            }
        }

        /// <summary>
        /// 显示
        /// </summary>
        public static void Show()
        {
            if ( fps == null )
            {
                fps = new GameObject( "FPS" ).AddComponent<AFPSCounter>();

                fps.PaddingOffset = Vector2.zero;
                fps.hotKey = KeyCode.None;
                fps.fpsCounter.Anchor = CodeStage.AdvancedFPSCounter.Labels.LabelAnchor.UpperLeft;
                fps.memoryCounter.Anchor = CodeStage.AdvancedFPSCounter.Labels.LabelAnchor.UpperLeft;
                fps.deviceInfoCounter.Deactivate();
            }

            fps.OperationMode = OperationMode.Normal;
        }

        /// <summary>
        /// 切换显示
        /// </summary>
        public static void Toggle()
        {
            if ( fps == null )
            {
                Show();
                return;
            }

            if ( fps.OperationMode == OperationMode.Disabled )
            {
                fps.OperationMode = OperationMode.Normal;
            }
            else if ( fps.OperationMode == OperationMode.Normal )
            {
                fps.OperationMode = OperationMode.Disabled;
            }
        }
    }
}