/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/09 16:34
	File base:	CCoroutine.cs
	author:		Leo

	purpose:	协程管理类
                赋予非 MonoBehaviour 以调用协程的能力
*********************************************************************/

using System.Collections;
using UnityEngine;
using System;
using System.Collections.Generic;

namespace CoffeeBean
{
    /// <summary>
    /// 协程管理类
    /// 赋予非 MonoBehaviour 以调用协程的能力
    /// </summary>
    public class CCoroutine : CSingletonMono<CCoroutine>
    {
        /// <summary>
        /// 中止一个协程
        /// </summary>
        /// <param name="routine"></param>
        public static void BreakCoroutine( Coroutine Cor )
        {
            Inst.StopCoroutine( Cor );
        }

        /// <summary>
        /// 中止一个协程
        /// 连同里面的子协程一起中止
        /// </summary>
        /// <param name="ie"></param>
        public static void BreakIEnumeratorNested( IEnumerator ie )
        {
            var cur = ie;
            CLOG.I( "IEnumerator {0} will Stop", cur.ToString() );
            Inst.StopCoroutine( cur );

            while ( cur.Current is IEnumerator )
            {
                cur = cur.Current as IEnumerator;
                CLOG.I( "Inner IEnumerator {0} will Stop", cur.ToString() );
                Inst.StopCoroutine( cur );
            }
        }

        /// <summary>
        /// 延时做什么事
        /// </summary>
        /// <param name="time"></param>
        /// <param name="ac"></param>
        public static async void DelayToDo( float time, Action ac )
        {
            await new WaitForSeconds( time );
            ac?.Invoke();
        }

        /// <summary>
        /// 启动一个协程
        /// </summary>
        /// <param name="routine"></param>
        public static Coroutine RunCoroutine( IEnumerator routine )
        {
            return Inst.StartCoroutine( routine );
        }
    }
}