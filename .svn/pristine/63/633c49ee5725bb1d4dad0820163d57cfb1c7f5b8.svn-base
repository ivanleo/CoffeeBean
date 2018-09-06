using System;
using System.Collections;
using System.Text;
using UnityEngine;
using System.IO;

namespace CoffeeBean
{

    /// <summary>
    /// HTTP请求回调
    /// </summary>
    /// <param name = "isSuccess" > 是否成功 </ param >
    /// < param name="error"> 错误</param>
    /// <param name = "responseStr" > 回调字符串 </ param >
    public delegate void DelegateHttpRequest ( string responseStr );

    /// <summary>
    /// HTTP封装
    /// </summary>
    public partial class CJXMHttp : CSingletonMono<CJXMHttp>
    {
        //袁杰内网
        //private const string URL = "http://192.168.1.209/racingcar/car.php";

        //测试内网
        private const string URL = "http://192.168.1.117:8080/racingcar/car.php";

        /// <summary>
        /// 请求发送Post
        /// </summary>
        /// <param name="Action">请求字符串</param>
        /// <param name="Obj">对象</param>
        public void PostRequest ( string action, object Obj, DelegateHttpRequest Callback, bool NeedAnimation = true )
        {
            if ( NeedAnimation )
            {
                //VUI_Loading.Show();
            }

            //POST数据
            string JsonData = JsonUtility.ToJson ( Obj );

            //请求URL
            string ReqURL = string.Format ( "{0}?action={1}&id={2}", URL, action, UserData.Instance.JXMData.user_id );

            CLOG.I ( "== HTTP REQUEST ==" );
            CLOG.I ( "URL:{0}", ReqURL );
            CLOG.I ( "data:{0}\n", JsonData );

            StartCoroutine ( SendPost ( ReqURL, JsonData, Callback, NeedAnimation ) );
        }

        /// <summary>
        /// Post请求协程
        /// </summary>
        /// <param name="URL">URL</param>
        /// <param name="JsonData">post字符串</param>
        /// <returns></returns>
        private IEnumerator SendPost ( string URL, string JsonData, DelegateHttpRequest Callback, bool NeedAnimation )
        {
            //身体数据
            byte[] dataBytes = Encoding.UTF8.GetBytes ( JsonData );

            WWW Req = new WWW ( URL, dataBytes );
            yield return Req;

            CLOG.I ( "== HTTP RESPONSE ==" );
            CLOG.I ( "isDone:{0}", Req.isDone );
            CLOG.I ( "error:{0}", Req.error );
            CLOG.I ( "data:{0}\n", Req.text );

            Time.timeScale = 1f;

            try
            {
                if ( !Req.isDone )
                {
                    CLOG.E ( "Http请求失败，失败信息：Req.isDone ={0}", Req.isDone );
                    //VUI_CommonPop.Show ( "网络异常,请稍后重试", EPopupButtonType.OK_ONLY );
                }
                else
                {
                    if ( Req.error != null )
                    {
                        CLOG.E ( "Http请求失败，失败信息：Req.error = {0}", Req.error );
                        //VUI_CommonPop.Show ( "网络异常,请稍后重试", EPopupButtonType.OK_ONLY );
                    }
                    else
                    {
                        if ( Callback != null )
                        {
                            Callback ( Req.text );
                        }
                    }

                }

                Req.Dispose();

                if ( NeedAnimation )
                {
                    //VUI_Loading.Hide();
                }

            }
            catch ( Exception EX )
            {
                Time.timeScale = 1f;

                //VUI_Loading.Hide();

                CLOG.E ( EX.ToString() );

                //弹出错误说明框
                //VUI_CommonPop.Show ( "网络异常,请稍后重试", EPopupButtonType.OK_ONLY );
            }
        }


        /// <summary>
        /// 下载资源文件
        /// </summary>
        /// <param name="resUrl">资源url</param>
        /// <param name="callback">下载完成回调</param>
        public void DownloadRes ( string resUrl, string localPath, Action callback )
        {
            StartCoroutine ( DownloadResCot ( resUrl, localPath, callback ) );
        }

        /// <summary>
        /// 下载资源文件协程
        /// </summary>
        /// <param name="resUrl">资源url</param>
        /// <param name="localPath">本地路径</param>
        /// <param name="callback">下载完成回调</param>
        /// <returns></returns>
        IEnumerator DownloadResCot ( string resUrl, string localPath, Action callback )
        {
            WWW www = new WWW ( resUrl );
            yield return www;

            //生成文件
            Byte[] bt = www.bytes;

            CLocalizeData.WriteFile ( localPath, bt );

            if ( callback != null )
            {
                callback();
            }

        }

    }

}

