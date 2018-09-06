using UnityEngine;
using UnityEngine.UI;
using System.IO;
using System;
using System.Collections;
using System.Collections.Generic;

namespace CoffeeBean
{

    /// <summary>
    /// HTTP位置
    /// </summary>
    public enum EHttpLocation
    {
        /// <summary>
        /// 本地
        /// </summary>
        [CEnumDesc ( Desc = "本地" )]
        LOCAL,
        /// <summary>
        /// 远程
        /// </summary>
        [CEnumDesc ( Desc = "远程" )]
        REMOTE,

        UNKNOWN
    }

    /// <summary>
    /// HTTP请求完成时的回调
    /// 内涵加载的数据
    /// </summary>
    public delegate void DelegateHttpLoadComplete ( WWW HttpObject, bool isSuccess );

    /// <summary>
    /// HTTP请求时的回调
    /// 内涵加载进度
    /// </summary>
    public delegate void DelegateHttpLoading ( WWW HttpObject );

    /// <summary>
    /// URL Image加载完毕回调
    /// </summary>
    public delegate void DelegateURLImageLoadComplete ( bool isSuccess );


    /// <summary>
    /// HTTP任务队列
    /// 特点：主线程异步下载，不阻塞主线程
    /// <code>
    ///  <para>CHttp.Instance.CreateHttpLoader( "http://zhongwei-info.com/apk/MoneyFruit1.2_165_sign.apk", Loaded, Loading );</para>
    ///  <para>CHttp.Instance.CreateHttpLoader( "http://zhongwei-info.com/apk/wuhh1.4.apk ", Loaded, Loading );</para>
    /// </code>
    /// </summary>
    public partial class CHttpSequence : CSingletonMono<CHttpSequence>
    {
        /// <summary>
        /// 协程队列
        /// </summary>
        private Queue<IEnumerator> m_WorkQueue = new Queue<IEnumerator>();

        /// <summary>
        /// 是否进行下一个
        /// </summary>
        private bool DoNext = true;

        /// <summary>
        /// 每帧更新
        /// </summary>
        private void Update()
        {
            Work();
        }

        /// <summary>
        /// 工作
        /// </summary>
        private void Work()
        {
            if ( DoNext && m_WorkQueue.Count > 0 )
            {
                StartCoroutine ( m_WorkQueue.Dequeue() );
                DoNext = false;
            }
        }

        /// <summary>
        /// 创建一个HTTP下载任务
        /// </summary>
        /// <param name="URL"> URL </param>
        /// <param name="CompleteCallBack"> 完成回调 </param>
        /// <param name="LoadingCallBack"> 下载中回调 </param>
        /// <param name="EHT"> URL位置是本地还是网络 </param>
        public void CreateHttpLoader ( string URL, DelegateHttpLoadComplete CompleteCallBack = null, DelegateHttpLoading LoadingCallBack = null, EHttpLocation EHT = EHttpLocation.REMOTE )
        {
            m_WorkQueue.Enqueue ( StartLoad ( URL, CompleteCallBack, LoadingCallBack, EHT ) );
        }

        /// <summary>
        /// 开始读取
        /// </summary>
        /// <param name="URL"></param>
        /// <param name="CompleteCallBack"></param>
        /// <param name="LoadingCallBack"></param>
        /// <param name="EHT"> URL位置是本地还是网络 </param>
        /// <returns>协程</returns>
        private IEnumerator StartLoad ( string URL, DelegateHttpLoadComplete CompleteCallBack = null, DelegateHttpLoading LoadingCallBack = null, EHttpLocation EHT = EHttpLocation.REMOTE )
        {
            if ( EHT == EHttpLocation.LOCAL )
            {
                URL = "file://" + URL;
                CLOG.I ( "read local file uri={0}", URL );
            }
            else
            {
                CLOG.I ( "read remote file url={0}", URL );
            }

            yield return null;
            WWW HttpObj = new WWW ( URL );
            yield return StartCoroutine ( Loading ( HttpObj, CompleteCallBack, LoadingCallBack ) );
            HttpObj.Dispose();
        }

        /// <summary>
        /// 读取中
        /// </summary>
        /// <returns></returns>
        private IEnumerator Loading ( WWW HttpObj, DelegateHttpLoadComplete CompleteCallBack = null, DelegateHttpLoading LoadingCallBack = null )
        {

            while ( !HttpObj.isDone )
            {
                if ( LoadingCallBack != null )
                {
                    LoadingCallBack ( HttpObj );
                }
                yield return null;
            }

            DoNext = true;

            if ( HttpObj.error != null )
            {
                CLOG.W ( "Http Loading Error: URL:{0} Error:{1}", HttpObj.url, HttpObj.error );
                if ( CompleteCallBack != null )
                {
                    CompleteCallBack ( HttpObj, false );
                }
            }
            else
            {
                if ( CompleteCallBack != null )
                {
                    CompleteCallBack ( HttpObj, true );
                }
            }
        }

    }


    /// <summary>
    /// Image的URL Loader
    /// 加载的图片会自动以MD5的形式保存本地
    /// 下次加载会直接读取本地文件提高速度
    /// 自动加入队列
    /// 每次只读一张图
    /// <code>
    ///     Image image = .......
    ///     <para>image.LoadURLImage( "http://61.183.69.235:7001/hx/uploadFiles/default/subject/20151019081139869433.jpg" );</para>
    /// </code>
    /// </summary>
    public static class CImageURLLoader
    {
        /// <summary>
        /// Image this扩展，读取图片
        /// </summary>
        /// <param name="image">Image组件</param>
        /// <param name="URL">网络URL头像地址</param>
        /// <param name="autoSize">自动尺寸</param>
        /// <param name="LoadCompleteCallBack">加载完毕回调</param>
        /// <param name="OnLoading">加载中回调</param>
        public static void LoadURLImage ( this Image image,
                                          string URL,
                                          bool autoSize = false,
                                          DelegateURLImageLoadComplete LoadCompleteCallBack = null,
                                          DelegateHttpLoading OnLoading = null )
        {
            if ( URL == null || URL == "" )
            {
                return;
            }

            //文件类型
            string TargetFileType = GetFileType ( URL );

            //得到文件名
            string TargetFileName = GetMD5HeadName ( URL ) + "." + TargetFileType;

            //本地文件名，用于判断文件是否存在
            string LoacalFileName = Application.persistentDataPath + "/" + TargetFileName;

            //判断本地文件是否存在
            FileInfo FI = new FileInfo ( LoacalFileName );

            //存在就读取本地文件，不存在就读取网络文件
            EHttpLocation EHT = FI.Exists ? EHttpLocation.LOCAL : EHttpLocation.REMOTE;

            //读取完毕回调
            DelegateHttpLoadComplete OnComplete = ( WWW HttpObj, bool isSuccess ) =>
            {
                try
                {
                    //加载失败时的回调
                    if ( !isSuccess )
                    {
                        if ( LoadCompleteCallBack != null )
                        {
                            LoadCompleteCallBack ( isSuccess );
                        }

                        return;
                    }

                    //加载成功，但是头像已销毁
                    if ( image == null )
                    {
                        return;
                    }

                    //得到Texture
                    Texture2D texture = HttpObj.texture;

                    if ( texture == null )
                    {
                        return;
                    }

                    if ( autoSize )
                    {
                        image.rectTransform.sizeDelta = new Vector2 ( texture.width, texture.height );
                    }

                    //设置Image图片精灵
                    image.sprite = Sprite.Create ( texture, new Rect ( 0, 0, texture.width, texture.height ), Vector2.zero );

                    //只有远程文件才需要保存
                    if ( EHT == EHttpLocation.REMOTE )
                    {
                        //得到字节流
                        byte[] ImageData = null;

                        if ( TargetFileType == "jpg" || TargetFileType == "jpeg" )
                        {
                            ImageData = texture.EncodeToJPG();
                        }
                        else if ( TargetFileType == "png" )
                        {
                            ImageData = texture.EncodeToPNG();
                        }

                        //保存路径
                        string SaveURl = Application.persistentDataPath + "/" + TargetFileName;
                        CLOG.I ( "read http remote data complete , Start save image to {0}!", SaveURl );

                        //保存文件
                        if ( ImageData != null )
                        {
                            File.WriteAllBytes ( SaveURl, ImageData );
                        }
                        else
                        {
                            CLOG.E ( "Write file {0} error!", SaveURl );
                        }
                    }

                    //执行回调
                    if ( LoadCompleteCallBack != null )
                    {
                        LoadCompleteCallBack ( isSuccess );
                    }
                }
                catch ( Exception ex )
                {
                    CLOG.I ( "**********  don't warry it's OK  ************" );
                    CLOG.E ( ex.ToString() );
                    CLOG.I ( "*********************************************" );
                }

            };

            //创建HTTP加载器
            if ( EHT == EHttpLocation.REMOTE )
            {
                //加载网络头像
                CHttpSequence.Instance.CreateHttpLoader ( URL, OnComplete, OnLoading, EHT );
            }
            else if ( EHT == EHttpLocation.LOCAL )
            {
                //加载本地文件
                CHttpSequence.Instance.CreateHttpLoader ( LoacalFileName, OnComplete, OnLoading, EHT );
            }
        }


        /// <summary>
        /// 得到网络路径的文件路径
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static string GetMD5HeadName ( string URL )
        {
            return URL.MD5();
        }

        /// <summary>
        /// 得到文件类型
        /// </summary>
        /// <param name="URL"></param>
        /// <returns></returns>
        private static string GetFileType ( string URL )
        {
            int LastSlash = URL.LastIndexOf ( '.' );
            return URL.Substring ( LastSlash + 1 );
        }

    }
}
