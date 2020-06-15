/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/11 10:21
	File base:	CMissionSequence.cs
	author:		Leo

	purpose:	任务队列

                CMissionSequence Seq = CMissionSystem.CreateSequence();
                Seq.AppendInteval ( 10f );
                Seq.Append ( new Mission_4() );
                Seq.Start();

*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using UnityEngine;
using System.Runtime.InteropServices;

namespace CoffeeBean
{
    /// <summary>
    /// 任务序列
    /// </summary>
    public class CTaskQueue : CTask
    {
        /// <summary>
        /// 任务队列
        /// </summary>
        private Queue<CTask> m_MissionQueue = new Queue<CTask>();

        /// <summary>
        /// 当前处理的任务
        /// </summary>
        private CTask m_NowExeMission = null;

        /// <summary>
        /// 增加一个简单任务到队列
        /// </summary>
        /// <param name=""></param>
        public void Append( CTask task )
        {
            var heap = CSubTaskHeap.Create(task);
            m_MissionQueue.Enqueue( heap );
        }

        /// <summary>
        /// 增加一个函数调用
        /// </summary>
        public void AppendCallFunc( Action Callfunc )
        {
            var callTask = CSubTaskCallFunc.Create(Callfunc);
            m_MissionQueue.Enqueue( callTask );
        }

        /// <summary>
        /// 增加一个延时
        /// </summary>
        /// <param name="WaitTime"></param>
        public void AppendInteval( float WaitTime )
        {
            var waitTask = CSubTaskInteval.Create( WaitTime );
            m_MissionQueue.Enqueue( waitTask );
        }

        /// <summary>
        /// 添加一个同步任务到队列
        /// </summary>
        /// <param name="Mission"></param>
        public void Join( CTask Mission )
        {
            if ( m_MissionQueue.Count == 0 )
            {
                Debug.LogWarning( $"TaskQueue:{Name} has no taskheap to join!!!" );
                return;
            }

            var last = m_MissionQueue.Last();
            if ( last is CSubTaskHeap )
            {
                ( last as CSubTaskHeap ).Join( Mission );
            }
            else
            {
                Debug.LogWarning( $"TaskQueue:{Name} can not join an task to the last queue where is not a taskheap" );
            }
        }

        /// <summary>
        /// 执行下一个任务
        /// </summary>
        public override void OnStart()
        {
            ExecuteNextMission();
        }

        /// <summary>
        /// 每帧刷新
        /// 如果本任务序列被执行完毕了，则返回true
        /// 未执行完毕，则返回false
        /// </summary>
        public override bool Update()
        {
            if ( m_NowExeMission != null )
            {
                // 当前任务运行完毕
                if ( m_NowExeMission.IsRunning && m_NowExeMission.Update() )
                {
                    // 执行完成接口
                    m_NowExeMission.Finish();
                    // 检查是否需要下一个任务
                    if ( m_MissionQueue.Count == 0 )
                    {
                        return true;
                    }
                    else
                    {
                        ExecuteNextMission();
                    }
                }
            }

            return false;
        }

        /// <summary>
        /// 执行下一个任务
        /// </summary>
        private void ExecuteNextMission()
        {
            m_NowExeMission = m_MissionQueue.Dequeue();

#if SHOW_TASK_LOG
            CLOG.I( "task",$"TaskQueue:{Name} Ready To Start Next Task:{m_NowExeMission.Name}");
#endif
            m_NowExeMission.Run( false );
        }
    }

    #region 子任务 Action

    /// <summary>
    /// Action 子任务
    /// </summary>
    internal class CSubTaskCallFunc : CTask
    {
        public Action CallBack { get; set; }

        private CSubTaskCallFunc()
        {
        }

        /// <summary>
        /// 创建一个Action任务
        /// </summary>
        /// <param name="CallFunc"></param>
        /// <returns></returns>
        public static CSubTaskCallFunc Create( Action CallFunc )
        {
            return new CSubTaskCallFunc() { CallBack = CallFunc };
        }

        /// <summary>
        /// 结束时
        /// </summary>
        public override void OnFinish()
        {
            CallBack?.Invoke();
        }

        public override bool Update()
        {
            return true;
        }
    }

    #endregion 子任务 Action

    #region 子任务堆

    /// <summary>
    /// 子任务堆
    /// 一堆可一起执行的任务
    /// </summary>
    internal class CSubTaskHeap : CTask
    {
        /// <summary>
        /// 同步执行的任务堆
        /// </summary>
        private List<CTask> Tasks = new List<CTask>();

        private CSubTaskHeap()
        {
        }

        /// <summary>
        /// 创建子任务堆
        /// </summary>
        /// <param name="firstTask"></param>
        /// <returns></returns>
        public static CSubTaskHeap Create( CTask firstTask = null )
        {
            var heap = new CSubTaskHeap();
            heap.Join( firstTask );
            return heap;
        }

        /// <summary>
        /// 添加一个同步任务到队列
        /// </summary>
        /// <param name="Mission"></param>
        public void Join( CTask task )
        {
            if ( task != null )
                Tasks.Add( task );
        }

        /// <summary>
        /// 运行任务堆
        /// </summary>
        /// <param name="inSystem"></param>
        public override void Run( bool inSystem = false )
        {
            base.Run( inSystem );

            for ( int i = 0; i < Tasks.Count; i++ )
            {
                Tasks[i].Run( false );
            }
        }

        /// <summary>
        /// 检查任务堆是否完成
        /// </summary>
        public override bool Update()
        {
            for ( int i = 0; i < Tasks.Count; i++ )
            {
                if ( Tasks[i].IsRunning && Tasks[i].Update() )
                {
                    Tasks[i].Finish();
                }
            }

            for ( int i = 0; i < Tasks.Count; i++ )
            {
                if ( !Tasks[i].HasFinish )
                {
                    return false;
                }
            }

            return true;
        }
    }

    #endregion 子任务堆

    #region 子任务 延时

    /// <summary>
    /// 延时子任务
    /// </summary>
    internal class CSubTaskInteval : CTask
    {
        /// <summary>
        /// 延时
        /// </summary>
        private float m_DelayTime = 0f;

        /// <summary>
        /// 当前经过时间
        /// </summary>
        private float m_NowTime = 0f;

        private CSubTaskInteval()
        {
        }

        /// <summary>
        /// 创建延时子任务
        /// </summary>
        /// <param name="DelayTime"></param>
        /// <returns></returns>
        public static CSubTaskInteval Create( float DelayTime )
        {
            return new CSubTaskInteval() { m_DelayTime = DelayTime };
        }

        /// <summary>
        /// 更新状态
        /// </summary>
        public override bool Update()
        {
            m_NowTime += Time.deltaTime;
            return m_NowTime >= m_DelayTime;
        }
    }

    #endregion 子任务 延时
}