/********************************************************************
    created:    2018/06/11
    created:    11:6:2018   15:28
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CCoroutineManager.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CCoroutineManager
    file ext:   cs
    author:     Leo

    purpose:    协程管理类
                赋予非 MonoBehaviour 以调用协程的能力
*********************************************************************/
using System.Collections;
using UnityEngine;
using System;

namespace CoffeeBean
{
    /// <summary>
    /// 协程管理类
    /// 赋予非 MonoBehaviour 以调用协程的能力
    /// </summary>
    public class CCoroutineManager: CSingletonMono<CCoroutineManager>
    {

        /// <summary>
        /// 启动一个协程
        /// </summary>
        /// <param name="routine"></param>
        public Coroutine DoCoroutine ( IEnumerator routine )
        {
            return StartCoroutine ( routine ) ;
        }

        /// <summary>
        /// 等待一段时间做一件事
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="ToDo"></param>
        public void DelayToDo ( float waitTime, System.Action ToDo )
        {
            StartCoroutine ( Delay ( waitTime, ToDo ) );
        }

        /// <summary>
        /// 等待一段时间
        /// </summary>
        /// <param name="waitTime"></param>
        /// <param name="ToDo"></param>
        /// <returns></returns>
        private IEnumerator Delay ( float waitTime, Action ToDo )
        {
            yield return new WaitForSeconds ( waitTime );

            if ( ToDo != null )
            {
                ToDo();
            }
        }

    }
}
