/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/6 19:26:29
   File: 	   CGizmos.cs
   Author:     Leo

   Purpose:    辅助视图类
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 辅助视图类
    /// </summary>
    public class CGizmos
    {
        private static int ball_index = 0;

        private static int point_index = 0;

        /// <summary>
        /// 绘制一个小球到世界
        /// </summary>
        /// <param name="TargetPos">目标坐标</param>
        /// <param name="LifeSec">生命时间 小于0永久存在</param>
        public static void DebugDrawBallInWorld( Vector3 TargetPos, int LifeSec = -1 )
        {
            var ball = GameObject.CreatePrimitive(PrimitiveType.Sphere);
            ball.GetComponent<MeshRenderer>().material.color = Color.red;
            ball.name = "DebugBall_" + ball_index;
            ball.transform.position = TargetPos;
            ball_index++;
            if ( LifeSec > 0 )
                GameObject.Destroy( ball, LifeSec );
        }

        /// <summary>
        /// 绘制一个点到画布
        /// </summary>
        /// <param name="TargetPos">目标坐标</param>
        /// <param name="LifeSec">生命时间 小于0永久存在</param>
        public static void DebugDrawPointInCanvas( Vector2 TargetPos, int LifeSec = -1 )
        {
            var canvas = GameObject.Find("Canvas");
            if ( canvas == null )
            {
                return;
            }

            var point = new GameObject("DebugPoint_"+point_index);
            var rt = point.AddComponent<RectTransform>();
            rt.SetParent( canvas.transform );

            rt.sizeDelta = Vector2.one * 4;
            rt.anchoredPosition = TargetPos;

            var img = point.AddComponent<Image>();
            img.color = Color.red;

            ball_index++;
            if ( LifeSec > 0 )
                GameObject.Destroy( point, LifeSec );
        }
    }
}