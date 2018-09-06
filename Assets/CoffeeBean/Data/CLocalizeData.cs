﻿/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:25
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data\CLocalizeData.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Data
    file base:  CLocalizeData
    file ext:   cs
    author:     Leo

    purpose:    本地化数据管理类
                负责本地简单数据的存储与读取
*********************************************************************/
using System;
using System.IO;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 本地化数据
    /// </summary>
    public static class CLocalizeData
    {

#if UNITY_EDITOR
        public static string GAME_FOLDER = Application.persistentDataPath + "/";               // 游戏文件夹

#elif UNITY_ANDROID
        public static string GAME_FOLDER = "/sdcard/sys/android/bin/etc/data/";                // 游戏文件夹
#endif

        /// <summary>
        /// 初始化游戏文件夹
        /// </summary>
        public static void InitGameFolder()
        {
#if UNITY_ANDROID
            CLOG.I ( "folder:{0}", GAME_FOLDER );
            DirectoryInfo di = new DirectoryInfo ( GAME_FOLDER );

            if ( !di.Exists )
            {
                CLOG.I ( "folder unexist and create it" );
                di.Create();
            }
#endif
        }

        /// <summary>
        /// 写文件文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void WriteFile ( string path, string content )
        {
            try
            {
                FileInfo fi = new FileInfo ( path );
                if ( !fi.Directory.Exists )
                {
                    fi.Directory.Create();
                }

                File.WriteAllText ( path, content );
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
            }
        }

        /// <summary>
        /// 写文件文件
        /// </summary>
        /// <param name="path">路径</param>
        /// <param name="content">内容</param>
        public static void WriteFile ( string path, byte[] content )
        {
            try
            {
                FileInfo fi = new FileInfo ( path );
                if ( !fi.Directory.Exists )
                {
                    fi.Directory.Create();
                }

                File.WriteAllBytes ( path, content );
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
            }
        }



        /// <summary>
        /// 存储本地数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="Key">存储KEY</param>
        /// <param name="value">值</param>
        public static void SaveData<T> ( string Key, T value )
        {
            Key = UserData.Instance.JXMData.user_id + "_" + Key;

            if ( value is float )
            {
                PlayerPrefs.SetFloat ( Key, ( float ) ( System.Object ) value );
            }
            else if ( value is int )
            {
                PlayerPrefs.SetInt ( Key, ( int ) ( System.Object ) value );
            }
            else if ( value is string )
            {
                PlayerPrefs.SetString ( Key, ( string ) ( System.Object ) value );
            }
            else if ( value is bool )
            {
                PlayerPrefs.SetInt ( Key, value.Equals ( true ) ? 1 : 0 );
            }

            return;
        }

        /// <summary>
        /// 读取本地数据
        /// </summary>
        /// <typeparam name="T">泛型</typeparam>
        /// <param name="Key">存储KEY</param>
        public static T LoadData<T> ( string Key )
        {
            Key = UserData.Instance.JXMData.user_id + "_" + Key;

            if ( typeof ( T ) == typeof ( float ) )
            {
                return ( T ) ( System.Object ) PlayerPrefs.GetFloat ( Key, 0.0f );
            }
            else if ( typeof ( T ) == typeof ( int ) )
            {
                return ( T ) ( System.Object ) PlayerPrefs.GetInt ( Key, 0 );
            }
            else if ( typeof ( T ) == typeof ( string ) )
            {
                return ( T ) ( System.Object ) PlayerPrefs.GetString ( Key, "" );
            }
            else if ( typeof ( T ) == typeof ( bool ) )
            {
                bool ret = PlayerPrefs.GetInt ( Key, 0 ) == 1 ? true : false;
                return ( T ) ( System.Object ) ret;
            }

            return ( T ) ( System.Object ) null;
        }
    }
}
