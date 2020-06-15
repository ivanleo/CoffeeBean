/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 17:27
	File base:	CHttp.cs
	author:		Leo

	purpose:	HTTP任务
                提供HTTP请求的封装
*********************************************************************/

using System;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Text;
using System.Threading.Tasks;
using Assets.CoffeeBean.Scripts.Http;
using UnityEngine;
using UnityEngine.Networking;

namespace CoffeeBean
{
    public static class CHttp
    {
        /// <summary>
        /// 下载文件
        /// 并保存本地
        /// 保存在 Application.persistentDataPath + "/download/"
        /// </summary>
        /// <param name="URL"> URL </param>
        /// <param name="FileName"> 保存的文件名 </param>
        /// <param name="progress"> 下载中回调 </param>
        public static async Task<bool> DownLoad( string URL, string FileName, Action<UnityWebRequest> progress = null )
        {
            using ( UnityWebRequest uWeb = new UnityWebRequest( URL, UnityWebRequest.kHttpVerbGET ) )
            {
                CLOG.I( "http", "# HTTP #: ----- DownLoad -----" );
                CLOG.I( "http", $"# HTTP #: URL: {URL}" );

                uWeb.downloadHandler = new DownloadHandlerFile( CApp.Download_Path + FileName );
                var AsyncOpertion = uWeb.SendWebRequest();

                if ( progress != null )
                {
                    while ( !uWeb.isDone )
                    {
                        await new WaitForUpdate();
                        progress( uWeb );
                    }
                }

                await AsyncOpertion;

                CLOG.I( "http", "# HTTP #: ----- Download Response -----" );
                CLOG.I( "http", $"# HTTP #: error: {uWeb.error}" );

                uWeb.Dispose();

                if ( !uWeb.error.IsNullOrEmpty() )
                {
                    throw new HTTPException( uWeb.error );
                }

                CLOG.I( "http", $"# HTTP #: Save on: {CApp.Download_Path + FileName}" );
                CLOG.I( "http", "# HTTP #: Download Successful" );

                return true;
            }
        }

        /// <summary>
        /// GET请求
        /// </summary>
        /// <param name="URL"> URL </param>
        public static async Task<string> Get( string URL )
        {
            using ( UnityWebRequest uWeb = new UnityWebRequest( URL, UnityWebRequest.kHttpVerbGET ) )
            {
                CLOG.I( "http", "# HTTP #: ----- GET -----" );
                CLOG.I( "http", $"# HTTP #: URL: {URL}" );

                uWeb.downloadHandler = new DownloadHandlerBuffer();
                await uWeb.SendWebRequest();

                uWeb.Dispose();

                CLOG.I( "http", "# HTTP #: ----- GET Response -----" );
                CLOG.I( "http", $"# HTTP #: error: {uWeb.error}" );

                if ( !uWeb.error.IsNullOrEmpty() )
                {
                    throw new HTTPException( uWeb.error );
                }

                string ret = uWeb.downloadHandler?.text;
                CLOG.I( "http", $"# HTTP #: data: {ret}" );

                return ret;
            }
        }

        /// <summary>
        /// 加载远程/本地图片
        /// </summary>
        /// <param name="URL">地址</param>
        /// <param name="progress">进度</param>
        /// <returns></returns>
        public static async Task<Texture2D> LoadTexture( string URL, Action<UnityWebRequest> progress = null )
        {
            using ( UnityWebRequest uWeb = UnityWebRequestTexture.GetTexture( URL ) )
            {
                CLOG.I( "http", "# HTTP #: ----- Load Image -----" );
                CLOG.I( "http", $"# HTTP #: URL: {URL}" );

                var AsyncOpertion = uWeb.SendWebRequest();

                if ( progress != null )
                {
                    while ( !uWeb.isDone )
                    {
                        await new WaitForUpdate();
                        progress( uWeb );
                    }
                }

                await AsyncOpertion;
                uWeb.Dispose();

                CLOG.I( "http", "# HTTP #: ----- Load Image Response -----" );
                CLOG.I( "http", $"# HTTP #: error: {uWeb.error}" );

                if ( !uWeb.error.IsNullOrEmpty() )
                {
                    throw new HTTPException( uWeb.error );
                }

                var Texture = DownloadHandlerTexture.GetContent ( uWeb );

                return Texture;
            }
        }

        /// <summary>
        /// Post请求
        /// 立即进行异步请求可以直接进行等待
        /// </summary>
        /// <param name="URL">地址</param>
        /// <param name="sendData">发送的数据，一般为json</param>
        /// <returns></returns>
        public static async Task<string> Post( string URL, string sendData )
        {
            using ( UnityWebRequest uWeb = new UnityWebRequest( URL, UnityWebRequest.kHttpVerbPOST ) )
            {
                CLOG.I( "http", "----- POST -----" );
                CLOG.I( "http", $"URL: {URL}" );
                CLOG.I( "http", $"data: {sendData}" );

                // 身体数据
                byte[] dataBytes = Encoding.UTF8.GetBytes ( sendData );
                uWeb.uploadHandler = (UploadHandler)new UploadHandlerRaw( dataBytes );
                // 设置请求头
                uWeb.SetRequestHeader( "Content-Type", "application/json" );
                // 回复缓冲区
                uWeb.downloadHandler = new DownloadHandlerBuffer();

                await uWeb.SendWebRequest();

                CLOG.I( "http", "----- POST Response -----" );
                CLOG.I( "http", $"# HTTP #: error: {uWeb.error}" );

                if ( !uWeb.error.IsNullOrEmpty() )
                {
                    throw new HTTPException( uWeb.error );
                }

                var ret = uWeb.downloadHandler?.text;
                CLOG.I( "http", $"data: {ret}" );
                return ret;
            }
        }

        /// <summary>
        /// 使用Post队列请求
        /// 可以保证请求一个接一个的执行
        /// 需要提供回调函数
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="sendData"></param>
        /// <param name="callback"></param>
        public static void PostQueue( string URL, string sendData, Action<string> callback )
        {
            if ( callback == null )
            {
                CLOG.E( "http", "in post queue the callback must be not null!!" );
                return;
            }

            CHttpQueue.Inst.Add( URL, sendData, callback );
        }
    }

    /// <summary>
    /// HTTP异常
    /// </summary>
    [Serializable]
    public class HTTPException : ApplicationException
    {
        public HTTPException( string message )
            : base( message ) { }
    }
}