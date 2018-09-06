using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using DG.Tweening.Core;
using DG.Tweening.Plugins.Core.PathCore;
using DG.Tweening.Plugins.Options;
using DG.Tweening;
using System;

namespace CoffeeBean
{
    /// <summary>
    /// 一次贝塞尔数据类
    /// </summary>
    [Serializable]
    public class CBezierCubic
    {
        public Vector3 start_pos;
        public Vector3 control_pos;
        public Vector3 end_pos;
        public int sample_count;
        public CBezierCubic Clone()
        {
            CBezierCubic CBC = new CBezierCubic();
            CBC.start_pos = start_pos;
            CBC.control_pos = control_pos;
            CBC.end_pos = end_pos;
            CBC.sample_count = sample_count;

            return CBC;
        }
    }

    /// <summary>
    /// 贝塞尔工具类
    /// 提供一次，二次贝塞尔采样，取点的功能
    /// </summary>
    public static class CBezierUtils
    {
        /// <summary>
        /// 执行一次贝塞尔运动
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="CBC">一次贝塞尔相关信息</param>
        /// <param name="duration">持续时间</param>
        /// <param name="pathType">路径类型</param>
        /// <param name="pathMode">路径模式</param>
        /// <param name="resolution">分辨率</param>
        /// <param name="gizmoColor">颜色</param>
        /// <returns></returns>
        public static TweenerCore<Vector3, Path, PathOptions> DOBezierCubic ( this Rigidbody target, CBezierCubic CBC, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null )
        {
            Vector3[] path = SampleBeizer ( CBC.start_pos, CBC.control_pos, CBC.end_pos, CBC.sample_count );
            return target.DOPath ( path, duration, pathType, pathMode, resolution, gizmoColor );
        }

        /// <summary>
        /// 执行一次贝塞尔运动
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="CBC">一次贝塞尔相关信息</param>
        /// <param name="duration">持续时间</param>
        /// <param name="pathType">路径类型</param>
        /// <param name="pathMode">路径模式</param>
        /// <param name="resolution">分辨率</param>
        /// <param name="gizmoColor">颜色</param>
        /// <returns></returns>
        public static TweenerCore<Vector3, Path, PathOptions> DOBezierCubic ( this Transform target, CBezierCubic CBC, float duration, PathType pathType = PathType.Linear, PathMode pathMode = PathMode.Full3D, int resolution = 10, Color? gizmoColor = null )
        {
            Vector3[] path = SampleBeizer ( CBC.start_pos, CBC.control_pos, CBC.end_pos, CBC.sample_count );
            return target.DOPath ( path, duration, pathType, pathMode, resolution, gizmoColor );
        }

        /// <summary>
        /// 根据T值，计算贝塞尔曲线上面相对应的点
        /// </summary>
        /// <param name="t"></param>T值
        /// <param name="p0"></param>起始点
        /// <param name="p1"></param>控制点
        /// <param name="p2"></param>目标点
        /// <returns></returns>根据T值计算出来的贝赛尔曲线点
        public static Vector3 CalculateCubicBezierPoint ( float time, Vector3 startPos, Vector3 controlPos, Vector3 endPos )
        {
            float t = CMath.Clamp ( time, 0f, 1f );
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
        /// <param name="startPoint"></param>起始点
        /// <param name="controlPoint"></param>控制点
        /// <param name="endPoint"></param>目标点
        /// <param name="sampleCount"></param>采样点的数量
        /// <returns></returns>存储贝塞尔曲线点的数组
        public static Vector3[] SampleBeizer ( Vector3 startPoint, Vector3 controlPoint, Vector3 endPoint, int sampleCount )
        {
            Vector3[] path = new Vector3[sampleCount];
            for ( int i = 0; i < sampleCount; i++ )
            {
                float t = i / ( float ) ( sampleCount - 1 );
                Vector3 point = CalculateCubicBezierPoint ( t, startPoint, controlPoint, endPoint );
                path[i - 1] = point;
            }
            return path;
        }
    }

}
