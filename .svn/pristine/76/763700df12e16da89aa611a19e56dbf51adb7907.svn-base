/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:53
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Utils\CUtilMemory.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Utils
    file base:  CUtilMemory
    file ext:   cs
    author:     Leo

    purpose:    内存工具类
*********************************************************************/
using System;
using System.Runtime.InteropServices;

namespace CoffeeBean
{
    /// <summary>
    /// 内存工具
    /// </summary>
    public class CMemory
    {
        /// <summary>
        /// 获得对象内存地址
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>内存地址字符串</returns>
        public static string GetMemoryAddress( object obj ) //  获取引用类型的内存地址方法
        {
            GCHandle h = GCHandle.Alloc( obj, GCHandleType.WeakTrackResurrection );
            IntPtr addr = GCHandle.ToIntPtr( h );
            return "0x" + addr.ToString( "X" );
        }
    }
}
