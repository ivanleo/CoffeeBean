/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/7 21:08:16
   File: 	    CHttpQueue.cs
   Author:     Leo

   Purpose:
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeBean;

namespace Assets.CoffeeBean.Scripts.Http
{
    /// <summary>
    /// Http队列
    /// </summary>
    public class CHttpQueue : CSingleton<CHttpQueue>
    {
        /// <summary>
        /// 是否正在执行
        /// </summary>
        private bool isRunning = false;

        /// <summary>
        /// HTTP队列
        /// </summary>
        private Queue<PostItem> PostQueue = new Queue<PostItem>();

        /// <summary>
        /// 添加任务
        /// </summary>
        public void Add( string uRL, string sendData, Action<string> callback )
        {
            PostQueue.Enqueue( new PostItem() { URL = uRL, data = sendData, callback = callback } );

            if ( !isRunning )
            {
                StartQueue();
            }
        }

        /// <summary>
        /// 开始队列
        /// </summary>
        private async void StartQueue()
        {
            isRunning = true;

            while ( isRunning )
            {
                if ( PostQueue.Count == 0 )
                {
                    isRunning = false;
                    return;
                }

                try
                {
                    var pi = PostQueue.Dequeue();
                    var rsp = await CHttp.Post( pi.URL, pi.data );
                    pi.callback.Invoke( rsp );
                }
                catch ( HTTPException ex )
                {
                    CLOG.E( "http", ex.ToString() );
                }
            }
        }
    }

    /// <summary>
    /// Post队列项
    /// </summary>
    internal class PostItem
    {
        /// <summary>
        /// 回调函数
        /// </summary>
        public Action<string> callback;

        /// <summary>
        /// 请求数据
        /// </summary>
        public string data;

        /// <summary>
        /// 请求地址
        /// </summary>
        public string URL;
    }
}