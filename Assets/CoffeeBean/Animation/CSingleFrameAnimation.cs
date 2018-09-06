/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:31
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CFrameAnimation.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CFrameAnimation
    file ext:   cs
    author:     Leo

    purpose:    单状态序列帧动画封装
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 单状态序列帧动画封装
    /// 只提供基本控制
    /// 不提供状态切换
    /// </summary>
    public class CSingleFrameAnimation : CFrameAnimationBase
    {
        /// <summary>
        /// 动画数据
        /// </summary>
        [SerializeField]
        private  CAnimation AnimationData = null;

        /// <summary>
        /// 开始时
        /// </summary>
        private void Start()
        {
            if ( m_AutoPlayOnStart )
            {
                PlayAnimation ( m_IsLoop );
            }
        }

        /// <summary>
        /// 播放动画
        /// </summary>
        /// <param name="IsLoop">是否循环</param>
        public void PlayAnimation (  bool IsLoop = false )
        {
            //如果在播放就停止
            if ( m_IsPlaying )
            {
                StopNowAnimation();
            }

            m_IsLoop = IsLoop;

            //设置播放头为第一帧
            m_PlayHeadIndex = 0;

            int AnimCount = AnimationData.Frames.Length;
            if ( AnimCount > 1 )
            {
                m_PlayCoroutine = StartCoroutine ( PlayHandler ( AnimationData ) );
            }
            else if ( AnimCount == 1 )
            {
                SetSprite ( AnimationData.Frames[0].SpFrame );
            }
            else
            {
                CLOG.E ( "none frame to play!" );
                return;
            }

            m_IsPlaying = true;
        }

        /// <summary>
        /// 设置特定动画每一帧的回调
        /// </summary>
        /// <param name="callback">回调</param>
        public void SetEveryFrameCallBack ( DelegateAnimationFrameEvent callback )
        {
            AnimationData.SetEveryFrameCallBack ( callback );
        }

        /// <summary>
        /// 添加特定动画特定帧回调
        /// </summary>
        /// <param name="FrameIndex">特定帧</param>
        /// <param name="callback"></param>
        public void AddFrameCallBack ( int FrameIndex, DelegateAnimationFrameEvent callback )
        {
            AnimationData.AddFrameCallBack ( FrameIndex, callback );
        }

        /// <summary>
        /// 移除特定动画特定帧的回调
        /// </summary>
        /// <param name="EAS">动画类型</param>
        /// <param name="FrameIndex">特定帧</param>
        public void DeleteFrameCallBack ( int FrameIndex )
        {
            AnimationData.DeleteFrameCallBack ( FrameIndex );
        }

        /// <summary>
        /// 移除特定动画所有帧回掉
        /// </summary>
        /// <param name="EAS">动画类型</param>
        public void DeleteAllFrameCallBack(  )
        {
            AnimationData.DeleteAllFrameCallBack( );
        }

#if UNITY_EDITOR
        private void OnValidate()
        {
            m_SR = GetComponent<SpriteRenderer>();
            m_Img = GetComponent<Image>();

            if ( m_SR == null && m_Img == null )
            {
                Debug.Log ( gameObject.name + " has no Image or SpriteRender Component! so the frame animation can not work!" );
                return;
            }
            SetSprite ( AnimationData.Frames[0].SpFrame );
        }
#endif

    }


}