/********************************************************************
    All Right Reserved By Leo
	created:	2019/01/23
	File:	    CExpandTransform.cs
	author:		Leo

	purpose:	Transform扩展
                提供更多方法
*********************************************************************/

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
        /// 销毁所有孩子
        /// </summary>
        /// <param name="Root">组件</param>
        /// <param name="InOneFrame">是否在当前帧立刻销毁</param>
        public static void DestroyAllChild( this Transform Root, bool InOneFrame = false )
        {
            if ( Root.childCount != 0 )
            {
                if ( InOneFrame )
                {
                    for ( int i = Root.childCount - 1; i >= 0; i-- )
                        GameObject.DestroyImmediate( Root.GetChild( i ).gameObject );
                }
                else
                {
                    for ( int i = Root.childCount - 1; i >= 0; i-- )
                        GameObject.Destroy( Root.GetChild( i ).gameObject );
                }
            }
        }

        /// <summary>
        /// 直接获得某个孩子身上的指定组件
        /// </summary>
        /// <typeparam name="T">泛型，组件类型</typeparam>
        /// <param name="target">this扩展</param>
        /// <param name="ChildName">孩子名字</param>
        /// <returns></returns>
        public static T FindChildComponent<T>( this Transform target, string ChildName ) where T : Component
        {
            Transform child = ChildName != null ? target.Find( ChildName ): target ;

            if ( child == null || child.Equals( null ) )
            {
                CLOG.E( $"in {target.name} can not find child {ChildName}" );
                return (T)null;
            }

            return child.GetComponent<T>();
        }

        /// <summary>
        /// 获得CanvasGroup来一起控制整个节点
        /// 如果该节点没有CanvasCroup则自动添加一个
        /// </summary>
        /// <param name="target"></param>
        /// <returns></returns>
        public static CanvasGroup GetCanvasGroup( this Transform target )
        {
            if ( target is RectTransform )
            {
                var cg = target.GetComponent<CanvasGroup>();
                if ( cg == null )
                {
                    cg = target.gameObject.AddComponent<CanvasGroup>();
                }

                return cg;
            }

            return null;
        }

        /// <summary>
        /// 一个物体是否在屏幕内
        /// </summary>
        /// <param name="obj">this扩展</param>
        /// <param name="cam">摄像机</param>
        /// <returns></returns>
        public static bool IsInScreen( this Transform obj, Camera cam = null )
        {
            if ( cam == null )
            {
                cam = Camera.main;
            }

            if ( cam == null )
            {
                return false;
            }

            Vector2 ScreenPos = cam.WorldToViewportPoint( obj.position );
            if ( ScreenPos.x < 0 || ScreenPos.x > 1 || ScreenPos.y < 0 || ScreenPos.y > 1 )
            {
                return false;
            }

            return true;
        }

        /// <summary>
        /// 变换归零
        /// </summary>
        /// <param name="target"></param>
        public static void Reset( this Transform target )
        {
            target.localRotation = Quaternion.identity;
            target.localScale = Vector3.one;
            target.localPosition = Vector3.zero;
        }

        /// <summary>
        /// 设置透明度
        /// </summary>
        /// <param name="target"></param>
        /// <param name="Alpha"></param>
        public static void SetAlpha( this RectTransform target, float Alpha )
        {
            var cg = target.GetCanvasGroup();
            cg.alpha = Alpha;
        }

        /// <summary>
        /// 直接设置X坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosX( this Transform target, float x )
        {
            var v3 = target.position;
            v3.x = x;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置XY坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXY( this Transform target, float x, float y )
        {
            var v3 = target.position;
            v3.x = x;
            v3.y = y;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置XYZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXYZ( this Transform target, float x, float y, float z )
        {
            var v3 = target.position;
            v3.x = x;
            v3.y = y;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置XZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosXZ( this Transform target, float x, float z )
        {
            var v3 = target.position;
            v3.x = x;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置Y坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosY( this Transform target, float y )
        {
            var v3 = target.position;
            v3.y = y;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置YZ坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosYZ( this Transform target, float y, float z )
        {
            var v3 = target.position;
            v3.y = y;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 直接设置Z坐标
        /// </summary>
        /// <param name="target"></param>
        /// <param name="x"></param>
        public static void SetPosZ( this Transform target, float z )
        {
            var v3 = target.position;
            v3.z = z;
            target.position = v3;
        }

        /// <summary>
        /// 令一个变换组件直接提到最高层
        /// </summary>
        /// <param name="target"></param>
        public static void SetTop( this Transform target )
        {
            target.SetSiblingIndex( target.parent.childCount );
        }
    }
}