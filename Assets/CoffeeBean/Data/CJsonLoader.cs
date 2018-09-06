/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:26
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data\CXmlData.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data
    file base:  CXmlData
    file ext:   cs
    author:     Leo

    purpose:    XML文件管理器
                负责XML文件的加载和缓存，方便使用
*********************************************************************/
using System;
using System.Collections.Generic;
using System.Xml.Linq;
using UnityEngine;
using System.IO;

namespace CoffeeBean
{
    /// <summary>
    /// JSON工具类
    /// </summary>
    public static class CJsonLoader
    {
        /// <summary>
        /// 从资源目录中读取json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T LoadJsonFromResources<T> ( string Path )
        {
            try
            {
                string jsonStr = CResourcesManager.LoadText ( Path );
                return ParseJsonString<T> ( jsonStr );
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
                return ( T ) ( object ) null;
            }
        }

        /// <summary>
        /// 从文件中读取Json
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="Path"></param>
        /// <returns></returns>
        public static T LoadJsonFromFile<T> ( string Path )
        {
            try
            {
                string jsonStr = File.ReadAllText ( Path );
                return ParseJsonString<T> ( jsonStr );
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
                return ( T ) ( object ) null;
            }
        }

        /// <summary>
        /// 解析Json字符串
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="JsonStr"></param>
        /// <returns></returns>
        public static T ParseJsonString<T> ( string JsonStr )
        {
            try
            {
                T obj = JsonUtility.FromJson<T> ( JsonStr );
                return obj;
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
                return ( T ) ( object ) null;
            }
        }

    }
}