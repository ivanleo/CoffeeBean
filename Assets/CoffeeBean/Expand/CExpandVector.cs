using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// Unity Vector2 Vector3 的扩展
    /// </summary>
    public static class CExpandVector
    {
        /// <summary>
        /// 通过屏幕坐标得到世界坐标
        /// </summary>
        /// <param name="ScreenPos">屏幕坐标</param>
        /// <returns></returns>
        public static Vector3 ScreenPos_To_WorldPos( Vector2 ScreenPos, float z = 0f )
        {
            Vector3 Pos = Camera.main.ScreenToWorldPoint( ScreenPos );
            Pos.z = z;
            return Pos;
        }

        /// <summary>
        /// 通过屏幕坐标得到世界坐标
        /// </summary>
        /// <param name="ScreenPos">屏幕坐标</param>
        /// <returns></returns>
        public static Vector3 ScreenPos_To_WorldPos( Vector3 ScreenPos )
        {
            Vector3 Pos = Camera.main.ScreenToWorldPoint( ScreenPos );
            return Pos;
        }


        /// <summary>
        /// 通过世界坐标得到屏幕坐标
        /// </summary>
        /// <param name="WorldPos">屏幕坐标</param>
        /// <returns></returns>
        public static Vector2 WorldPos_To_ScreenPos( Vector3 WorldPos )
        {
            return Camera.main.WorldToScreenPoint( WorldPos );
        }
    }
}
