/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/08 8:32
	Filee: 	    CMissionSystem.cs
	Author:		Leo

	Purpose:	异步任务系统
                一个高度抽象的任务系统
                任务完成什么用户自行设定
                传入具体的函数即可

                语法规则同DoTween

                // 队列任务的运行
                var seq = new CTaskQueue();
                seq.Append( new TaskTest1() );
                seq.AppendCallFunc( () => Debug.Log( "1111111111111" ) );
                seq.AppendInteval( 3f );
                seq.Append( new TaskTest2() );
                seq.AppendCallFunc( () => Debug.Log( "2222222222222" ) );
                seq.Run();

*********************************************************************/

using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 任务系统
    /// </summary>
    public class CTaskSystem : CSingletonMono<CTaskSystem>
    {
        /// <summary>
        /// 当前任务数
        /// </summary>
        [SerializeField]
        [CReadOnly]
        private int m_TaskCount = 0;

        /// <summary>
        /// 任务清单
        /// </summary>
        private List<CTask> m_Tasks = new List<CTask>();

        /// <summary>
        /// 当前任务数
        /// </summary>
        public int TaskCount => m_Tasks == null ? 0 : m_Tasks.Count;

        /// <summary>
        /// 创建任务队列
        /// </summary>
        /// <returns></returns>
        public void AddTask( CTask target )
        {
            m_Tasks.Add( target );
            m_TaskCount++;
        }

        /// <summary>
        /// 每帧执行
        /// </summary>
        private void Update()
        {
            for ( int i = 0; i < m_Tasks.Count; i++ )
            {
                var task = m_Tasks[i];

                if ( task.HasFinish )
                {
                    m_Tasks.RemoveAt( i );
                    m_TaskCount--;
                    i--;
                }
                else if ( task.IsRunning && task.Update() )
                {
                    task.Finish();
                    m_Tasks.RemoveAt( i );
                    m_TaskCount--;
                    i--;
                }
            }
        }
    }
}