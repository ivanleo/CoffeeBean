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
        /// 包装Unity的随机数
        /// </summary>
        /// <param name="max">最大数</param>
        /// <param name="min">最小数,默认为0</param>
        /// <returns>随机数</returns>
        public static int Rand ( int max, int min = 0 )
        {
            if ( max < min )
            {
                return Random.Range ( max, min );
            }

            return Random.Range ( min, max );
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
        public static float Rand ( float max, float min = 0 )
        {
            if ( max < min )
            {
                return Random.Range ( max, min );
            }

            return Random.Range ( min, max );
        }


        /// <summary>
        /// 截断一个数字在一定范围
        /// </summary>
        /// <param name="Num">数字</param>
        /// <param name="Min">最小范围</param>
        /// <param name="Max">最大范围</param>
        /// <returns></returns>
        public static float Clamp ( float Num, float Min, float Max )
        {
            if ( Num < Min )
            {
                return Min;
            }
            if ( Num > Max )
            {
                return Max;
            }
            return Num;
        }

        /// <summary>
        /// 截断一个数字在一定范围
        /// </summary>
        /// <param name="Num">数字</param>
        /// <param name="Min">最小范围</param>
        /// <param name="Max">最大范围</param>
        /// <returns></returns>
        public static int Clamp ( int Num, int Min, int Max )
        {
            if ( Num < Min )
            {
                return Min;
            }
            if ( Num > Max )
            {
                return Max;
            }
            return Num;
        }

        /// <summary>
        /// 让一个角度标准化  归入 [0,360) 度之间
        /// </summary>
        /// <param name="Angle">角度</param>
        public static float MakeAngleNormalize ( float Angle )
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
        public static float ReMap ( float num, float min, float max, float newMin, float newMax )
        {
            if ( num <= min ) { return newMin; }
            if ( num >= max ) { return newMax; }

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
        public static int ReMap ( int num, int min, int max, int newMin, int newMax )
        {
            if ( num <= min ) { return newMin; }
            if ( num >= max ) { return newMax; }

            return ( num - min ) / ( max - min ) * ( newMax - newMin ) + newMin;
        }

        /// <summary>
        /// 得到一个概率是否命中
        /// </summary>
        /// <param name="Ratio">概率</param>
        /// <returns></returns>
        public static bool CanRatioBingo ( int Ratio, EPrecentType precentType = EPrecentType.PRECENT_10000 )
        {
            return UnityEngine.Random.Range ( 0, ( int ) precentType ) <= Ratio;
        }

        /// <summary>
        /// 随机打乱一个数组
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="array">泛型数组引用</param>
        public static void Shuffle<T> ( T[] array )
        {
            for ( int i = 0 ; i < array.Length ; ++i )
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
        public static void Shuffle<T> ( List<T> list )
        {
            for ( int i = 0 ; i < list.Count ; ++i )
            {
                int TargetPos = Rand ( list.Count );
                T temp = list[i];
                list[i] = list[TargetPos];
                list[TargetPos] = temp;
            }
        }


    }
}
