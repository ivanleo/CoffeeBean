/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/11 10:09
	File base:	CTask.cs
	author:		Leo

	purpose:	任务基类
                使用方法

                public class test_1 : CMission
                {
                    public int i = 0;
                    // 任务开始
                    public override void OnStart()
                    {
                        Debug.Log(i);
                    }

                    // 任务每帧执行
                    public override void Update()
                    {
                        Debug.Log(++i);
                        if(i >= 10)
                            Finish();
                    }

                      // 任务每帧执行
                    public override void OnFinish()
                    {
                        Debug.Log(999);
                    }
                }

                new test().Run();
                显示 0 1 2 3 4 5 6 7 8 9 10 999  最后一个999在 OnFinish 里输出的

*********************************************************************/

using System;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 简单任务
    /// </summary>

    public class CTask
    {
        /// <summary>
        /// 是否已完成
        /// </summary>
        public bool HasFinish { get; private set; }

        /// <summary>
        /// 任务状态
        /// </summary>
        public bool IsRunning { get; private set; }

        /// <summary>
        /// 任务名
        /// </summary>
        public string Name => ToString();

        /// <summary>
        /// 完成任务
        /// 任务调度器会在Update为true时自动调用本函数结束任务
        /// </summary>
        public virtual void Finish()
        {
            IsRunning = false;
            HasFinish = true;
            OnFinish();
            Debug.Log( $"Task:{Name} finished!" );
        }

        /// <summary>
        /// 结束时
        /// 子类需要重写来实现自己的任务
        /// </summary>
        public virtual void OnFinish() { }

        /// <summary>
        /// 开始时
        /// 子类需要重写来实现自己的任务
        /// </summary>
        public virtual void OnStart() { }

        /// <summary>
        /// 开始任务
        /// </summary>
        public virtual void Run( bool inSystem = true )
        {
            Debug.Log( $"Task:{Name} Start" );
            if ( inSystem )
                CTaskSystem.Inst.AddTask( this );

            IsRunning = true;
            HasFinish = false;
            OnStart();
        }

        /// <summary>
        /// 更新状态
        /// 返回true 代表该任务执行完毕
        /// 自动进入 OnFinish
        /// </summary>
        public virtual bool Update() { return false; }
    }
}