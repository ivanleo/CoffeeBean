/********************************************************************
	created:	2018/08/17
	created:	17:8:2018   10:52
	filename: 	D:\Work\CoffeeBean\Assets\CoffeeBean\Animation\CFrameData.cs
	file path:	D:\Work\CoffeeBean\Assets\CoffeeBean\Animation
	file base:	CFrameData
	file ext:	cs
	author:		Leo

	purpose:	序列帧动画数据类
*********************************************************************/
using System;
using System.Collections.Generic;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 帧回调
    /// </summary>
    /// <param name="frameIndex"></param>
    public delegate void DelegateAnimationFrameEvent ( int frameIndex );

    /// <summary>
    /// 帧的数据结构
    /// </summary>
    [Serializable]
    public class CFrame
    {
        /// <summary>
        /// 帧精灵
        /// </summary>
        public Sprite SpFrame;

        /// <summary>
        /// 帧延时
        /// </summary>
        public float SpInteval = 0.1f;
    }

    /// <summary>
    /// 动画结构
    /// 包含回调信息的帧集合
    /// </summary>
    [Serializable]
    public class CAnimation
    {
        /// <summary>
        /// 帧
        /// </summary>
        public CFrame[] Frames;

        /// <summary>
        /// 每帧回调
        /// </summary>
        public DelegateAnimationFrameEvent EveryFrameCallback = null;

        /// <summary>
        /// 特定帧回调
        /// </summary>
        public Dictionary<int, DelegateAnimationFrameEvent> FrameCallbacks = new Dictionary<int, DelegateAnimationFrameEvent>();

        /// <summary>
        /// 设置每一帧的回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void SetEveryFrameCallBack ( DelegateAnimationFrameEvent callback )
        {
            EveryFrameCallback = callback;
        }

        /// <summary>
        /// 添加帧回调
        /// </summary>
        /// <param name="FrameIndex">特定帧</param>
        /// <param name="callback"></param>
        public void AddFrameCallBack ( int FrameIndex, DelegateAnimationFrameEvent callback )
        {
            //替换
            if ( FrameCallbacks.ContainsKey ( FrameIndex ) )
            {
                FrameCallbacks[FrameIndex] = callback;
            }
            else
            {
                FrameCallbacks.Add ( FrameIndex, callback );
            }
        }

        /// <summary>
        /// 移除特定帧的回调
        /// </summary>
        /// <param name="FrameIndex">特定帧</param>
        public void DeleteFrameCallBack ( int FrameIndex )
        {
            //替换
            if ( FrameCallbacks.ContainsKey ( FrameIndex ) )
            {
                FrameCallbacks.Remove ( FrameIndex );
            }
        }

        /// <summary>
        /// 移除所有帧回掉
        /// </summary>
        public void DeleteAllFrameCallBack()
        {
            FrameCallbacks.Clear();
        }

    }
}

