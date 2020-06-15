/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 23:07
	File base:	CBezier.cs
	author:		Leo

	purpose:	贝塞尔曲线工具类

*********************************************************************/

using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 贝塞尔工具类 提供一次，二次贝塞尔采样，取点的功能
    /// </summary>
    public static class CBezierUtils
    {
        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="time">T值</param>
        /// <param name="startPos">起始点</param>
        /// <param name="controlPos">控制点</param>
        /// <param name="endPos">目标点</param>
        /// <returns>根据T值计算出来的贝赛尔曲线点</returns>
        public static Vector3 CalculateCubicBezierPointByTime( float time, Vector3 startPos, Vector3 controlPos, Vector3 endPos )
        {
            float t = Mathf.Clamp ( time, 0f, 1f );
            float u = 1 - time;
            float tt = time * time;
            float uu = u * u;

            Vector3 p = uu * startPos;
            p += 2 * u * t * controlPos;
            p += tt * endPos;

            return p;
        }

        /// <summary>
        /// 获取存储贝塞尔曲线点的数组
        /// </summary>
        /// <param name="startPoint">起始点</param>
        /// <param name="controlPoint">控制点</param>
        /// <param name="endPoint">目标点</param>
        /// <param name="sampleCount">采样点的数量</param>
        /// <returns>存储贝塞尔曲线点的数组</returns>
        public static Vector3[] SampleBeizer_One( Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int sampleCount )
        {
            Vector3[] path = new Vector3[sampleCount];

            for ( int i = 0; i < sampleCount; i++ )
            {
                float t = i / ( float ) ( sampleCount - 1 );
                Vector3 point = CalculateCubicBezierPointByTime ( t, startPoint, controlPoint, endPoint );
                path[i - 1] = point;
            }

            return path;
        }
    }
}