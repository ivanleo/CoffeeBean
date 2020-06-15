/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 20:17
	File base:	CMath.cs
	author:		Leo

	purpose:	数学库封装
                提供基本的数学方法封装
*********************************************************************/

using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 数学库
/// </summary>
namespace CoffeeBean
{
    /// <summary>
    /// 数学类
    /// </summary>
    public static class CMath
    {
        /// <summary>
        /// 角度转弧度 直接乘
        /// </summary>
        public const float Angle_2_Radian = 0.017453292519f;

        /// <summary>
        /// 弧度转角度 直接乘
        /// </summary>
        public const float Radian_2_Angle = 57.295779513082f;

        /// <summary>
        /// 得到一个概率是否命中
        /// </summary>
        /// <param name="Ratio">概率</param>
        /// <returns></returns>
        public static bool CanRatioBingo( int Ratio, EPrecentType precentType = EPrecentType.PRECENT_10000 )
        {
            return Random.Range( 0, (int)precentType ) <= Ratio;
        }

        /// <summary>
        /// 判断2个浮点数是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equal( float a, float b )
        {
            return ( a - b > -0.000001f ) && ( a - b ) < 0.000001f;
        }

        /// <summary>
        /// 判断2个浮点数是否相等
        /// </summary>
        /// <param name="a"></param>
        /// <param name="b"></param>
        /// <returns></returns>
        public static bool Equal( double a, double b )
        {
            return ( a - b > -0.000001d ) && ( a - b ) < 0.000001d;
        }

        /// <summary>
        /// 让一个角度标准化  归入 [0,360) 度之间
        /// </summary>
        /// <param name="Angle">角度</param>
        public static float MakeAngleNormalize( float Angle )
        {
            while ( Angle >= 360 )
            {
                Angle -= 360;
            }

            while ( Angle < 0 )
            {
                Angle += 360;
            }

            return Angle;
        }

        /// <summary>
        /// 包装Unity的随机数
        /// </summary>
        /// <param name="max">最大数</param>
        /// <param name="min">最小数,默认为0</param>
        /// <returns>随机数</returns>
        public static int Rand( int max, int min = 0 )
        {
            if ( max < min )
            {
                return Random.Range( max, min );
            }

            return Random.Range( min, max );
        }

        /// <summary>
        /// 返回0-1之间的一个随机数
        /// </summary>
        /// <returns>随机数</returns>
        public static float Rand()
        {
            return Random.value;
        }

        /// <summary>
        /// 包装Unity的随机数
        /// </summary>
        /// <param name="max">最大数</param>
        /// <param name="min">最小数,默认为0</param>
        /// <returns></returns>
        public static float Rand( float max, float min = 0 )
        {
            if ( max < min )
            {
                return Random.Range( max, min );
            }

            return Random.Range( min, max );
        }

        /// <summary>
        /// 把一个数字从当前的 min,max区间缩放到 newMin , newMax区间
        /// 如 区间 0-100 数字50
        /// 缩放区间倒 20-40 则返回数字30
        /// </summary>
        /// <param name="Num">要处理的数字</param>
        /// <param name="min">原缩放区间左值</param>
        /// <param name="max">原缩放区间右值</param>
        /// <param name="newMin">新区间左值</param>
        /// <param name="newMax">新区间右值</param>
        /// <returns></returns>
        public static float ReMap( float num, float min, float max, float newMin, float newMax )
        {
            if ( num <= min )
            {
                return newMin;
            }

            if ( num >= max )
            {
                return newMax;
            }

            return ( num - min ) / ( max - min ) * ( newMax - newMin ) + newMin;
        }

        /// <summary>
        /// 把一个数字从当前的 min,max区间缩放到 newMin , newMax区间
        /// 如 区间 0-100 数字50
        /// 缩放区间倒 20-40 则返回数字30
        /// </summary>
        /// <param name="Num">要处理的数字</param>
        /// <param name="min">原缩放区间左值</param>
        /// <param name="max">原缩放区间右值</param>
        /// <param name="newMin">新区间左值</param>
        /// <param name="newMax">新区间右值</param>
        /// <returns></returns>
        public static int ReMap( int num, int min, int max, int newMin, int newMax )
        {
            if ( num <= min )
            {
                return newMin;
            }

            if ( num >= max )
            {
                return newMax;
            }

            return ( num - min ) / ( max - min ) * ( newMax - newMin ) + newMin;
        }

        /// <summary>
        /// 随机打乱一个数组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">泛型数组引用</param>
        public static void Shuffle<T>( T[] array )
        {
            for ( int i = 0; i < array.Length; ++i )
            {
                int TargetPos = Rand ( array.Length );
                T temp = array[i];
                array[i] = array[TargetPos];
                array[TargetPos] = temp;
            }
        }

        /// <summary>
        /// 随机打乱一个List
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="List">泛型List引用</param>
        public static void Shuffle<T>( List<T> list )
        {
            for ( int i = 0; i < list.Count; ++i )
            {
                int TargetPos = Rand ( list.Count );
                T temp = list[i];
                list[i] = list[TargetPos];
                list[TargetPos] = temp;
            }
        }
    }
}