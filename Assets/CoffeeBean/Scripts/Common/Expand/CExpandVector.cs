/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/7 16:24:38
   File: 	   CExpandVector.cs
   Author:     Leo

   Purpose:    向量扩展类
*********************************************************************/

using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 向量扩展类
    /// </summary>
    public static class CExpandVector
    {
        /// <summary>
        /// 对于Vector2而言
        /// 也常常用作表示Size
        /// Height 等同于 Y
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float Height( this Vector2 source )
        {
            return source.x;
        }

        /// <summary>
        /// 对于Vector2而言
        /// 也常常用作表示Size
        /// Width 等同于 X
        /// </summary>
        /// <param name="source"></param>
        /// <returns></returns>
        public static float Width( this Vector2 source )
        {
            return source.x;
        }
    }
}