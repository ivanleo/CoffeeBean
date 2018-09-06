using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;
using UnityEngine.EventSystems;
using UnityEngine.UI;

namespace CoffeeBean
{
    public static class EEditorTools
    {
        /// <summary>
        /// 安全的得到画布
        /// </summary>
        /// <returns></returns>
        public static RectTransform SafeGetCanvas()
        {
            GameObject Canvas = GameObject.Find ( "Canvas" );
            if ( Canvas == null )
            {
                Canvas = new GameObject ( "Canvas" );
                Canvas.AddComponent<RectTransform>();
                Canvas cas = Canvas.AddComponent<Canvas>();
                cas.renderMode = RenderMode.ScreenSpaceOverlay;
                Canvas.AddComponent<CanvasScaler>();
                Canvas.AddComponent<GraphicRaycaster>();
            }

            GameObject evt = GameObject.Find ( "EventSystem" );

            if ( evt == null )
            {
                evt = new GameObject ( "EventSystem" );
                evt.AddComponent<EventSystem>();
                evt.AddComponent<StandaloneInputModule>();
            }

            return Canvas.transform as RectTransform;
        }
    }
}
