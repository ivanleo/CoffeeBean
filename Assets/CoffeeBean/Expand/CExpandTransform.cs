using DG.Tweening;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 变换组件扩展
    /// </summary>
    public static class CExpandTransform
    {
        /// <summary>
        /// 直接获得某个孩子身上的指定组件
        /// </summary>
        /// <typeparam name="T">泛型，组件类型</typeparam>
        /// <param name="target">this扩展</param>
        /// <param name="ChildName">孩子名字</param>
        /// <returns></returns>
        public static T FindChildComponent<T> ( this Transform target, string ChildName )
        {
            Transform child = target.Find ( ChildName );
            if ( child == null )
            {
                CLOG.E ( "in {0} can not find child {1}", target.name, ChildName );
                return ( T ) ( System.Object ) null;
            }

            return child.GetComponent<T>();
        }

        /// <summary>
        /// 直接设置X坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosX ( this Transform target, float x )
        {
            var v3 = target.position;
            v3.x = x;
            target.position = v3;
        }


        /// <summary>
        /// 直接设置Y坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosY ( this Transform target, float y )
        {
            var v3 = target.position;
            v3.y = y;
            target.position = v3;
        }


        /// <summary>
        /// 直接设置Z坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosZ ( this Transform target, float z )
        {
            var v3 = target.position;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置XY坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXY ( this Transform target, float x, float y )
        {
            var v3 = target.position;
            v3.x = x;
            v3.y = y;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置YZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosYZ ( this Transform target, float y, float z )
        {
            var v3 = target.position;
            v3.y = y;
            v3.z = z;
            target.position = v3;
        }


        /// <summary>
        /// 直接设置XZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXZ ( this Transform target, float x, float z )
        {
            var v3 = target.position;
            v3.x = x;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置XYZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXYZ ( this Transform target, float x, float y, float z )
        {
            var v3 = target.position;
            v3.x = x;
            v3.y = y;
            v3.z = z;
            target.position = v3;

        }


        /// <summary>
        /// 一个物体是否在屏幕内
        /// </summary>
        /// <param name="obj">this扩展</param>
        /// <param name="cam">摄像机</param>
        /// <returns></returns>
        public static bool IsInScreen ( this Transform obj, Camera cam = null )
        {
            if ( cam == null )
            {
                cam = Camera.main;
            }

            if ( cam == null )
            {
                return false;
            }

            Vector2 ScreenPos = cam.WorldToViewportPoint ( obj.position );
            return ScreenPos.x >= 0 && ScreenPos.x <= 1 && ScreenPos.y >= 0 && ScreenPos.y <= 1;
        }

        /// <summary>
        /// 缩放进入
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useTime"></param>
        public static void ScaleIn ( this Transform obj, float useTime = 0.5f, TweenCallback callback = null )
        {
            obj.localScale = Vector3.zero;
            obj.DOScale ( Vector3.one, useTime ).OnComplete ( callback );
        }

        /// <summary>
        /// 缩放进入
        /// </summary>
        /// <param name="obj"></param>
        /// <param name="useTime"></param>
        public static void ScaleOut ( this Transform obj, TweenCallback callback = null, float useTime = 0.5f  )
        {
            obj.DOScale ( Vector3.zero, useTime ).OnComplete ( callback );
        }

        /// <summary>
        /// 销毁所有孩子
        /// </summary>
        /// <param name="Root">组件</param>
        /// <param name="Quick">是否立刻销毁</param>
        public static void DestroyAllChild ( this Transform Root, bool Quick = false )
        {
            for ( int i = Root.childCount - 1 ; i >= 0 ; i-- )
            {
                if ( Quick )
                {
                    GameObject.DestroyImmediate ( Root.GetChild ( i ).gameObject );
                }
                else
                {
                    GameObject.Destroy ( Root.GetChild ( i ).gameObject );
                }
            }
        }

    }
}
