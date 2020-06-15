/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 23:35
	File base:	CSystem.cs
	author:		Leo

    purpose:    系统工具类

*********************************************************************/

using System;
using System.Runtime.InteropServices;

using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 系统工具
    /// </summary>
    public class CSystem
    {
        /// <summary>
        /// 设备信息
        /// </summary>
        private static DeviceInfo sysInfo = null;

        /// <summary>
        /// 设备信息
        /// </summary>
        public static DeviceInfo SysInfo
        {
            get
            {
                if ( sysInfo == null )
                {
                    sysInfo = GetDeviceInfo();
                }

                return sysInfo;
            }
        }

        /// <summary>
        /// 获取设备信息
        /// </summary>
        public static DeviceInfo GetDeviceInfo()
        {
            var di = new DeviceInfo();
            di.device_model = SystemInfo.deviceModel;
            di.device_type = SystemInfo.deviceType.ToString();
            di.device_unique_id = SystemInfo.deviceUniqueIdentifier;
            di.graphics_memory_size = SystemInfo.graphicsMemorySize;
            di.system_memory_size = SystemInfo.systemMemorySize;
            di.operating_system = SystemInfo.operatingSystem;
            return di;
        }

        /// <summary>
        /// 获得对象内存地址
        /// </summary>
        /// <param name="obj">对象</param>
        /// <returns>内存地址字符串</returns>
        public static string GetMemoryAddress( object obj )
        {
            //  获取引用类型的内存地址方法
            GCHandle h    = GCHandle.Alloc( obj, GCHandleType.WeakTrackResurrection );
            IntPtr   addr = GCHandle.ToIntPtr( h );
            return "0x" + addr.ToString( "X" );
        }
    }

    /// <summary>
    /// 设备信息
    /// </summary>
    [Serializable]
    public class DeviceInfo
    {
        public string device_model;         // 设备类型（Android,Apple等）
        public string device_type;          // 设备类型（手持，电脑等）
        public string device_unique_id;     // 备唯一标识
        public int    graphics_memory_size; // 显存单位（MB）
        public string operating_system;     // 操作系统
        public int    system_memory_size;   // 系统内存大小（MB）
    }
}