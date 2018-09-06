/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:31
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core\CFrameAnimation.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Core
    file base:  CFrameAnimation
    file ext:   cs
    author:     Leo

    purpose:    多状态序列帧动画封装
*********************************************************************/
using System;
using System.Collections;
using UnityEngine;
using UnityEngine.UI;


namespace CoffeeBean
{
    /// <summary>
    /// 动画状态
    /// </summary>
    [Serializable]
    public class CAnimatinState
    {
        public string StateName;
        public CAnimation StateAnimation;
    }

    /// <summary>
    /// 多状态序列帧动画封装
    /// 只提供基本控制
    /// 以及状态切换功能
    /// </summary>
    public class CMutiFrameAnimation : CFrameAnimationBase
    {
        /// <summary>
        /// 所有的状态动画数据
        /// </summary>
        [SerializeField]
        private CAnimatinState[] m_SAnimationDatas = null;

        /// <summary>
        /// 当前播放状态序号
        /// </summary>
        [ReadOnly]
        [SerializeField]
        private int m_NowPlayerStateIndex = 0;


        /// <summary>
        /// 开始时
        /// </summary>
        private void Start()
        {
            if ( m_SAnimationDatas == null || m_SAnimationDatas.Length == 0 )
            {
                CLOG.E ( "no animation to play!" );
                return;
            }

            if ( m_AutoPlayOnStart )
            {
                PlayAnimation ( m_NowPlayerStateIndex, m_IsLoop );
            }
        }

        /// <summary>
        /// 播放动画
        /// 通过状态名字播放
        /// </summary>
        /// <param name="StateName"></param>
        /// <param name="IsLoop"></param>
        public void PlayAnimation ( string StateName, bool IsLoop = false )
        {
            if ( m_SAnimationDatas == null || m_SAnimationDatas.Length == 0 )
            {
                CLOG.E ( "no animation to play!" );
                return;
            }

            PlayAnimation ( GetStateIndexByName ( StateName ), IsLoop );
        }

        /// <summary>
        /// 播放动画
        /// 通过状态序号播放
        /// </summary>
        /// <param name="IsLoop">是否循环</param>
        public void PlayAnimation ( int StateIndex,  bool IsLoop = false )
        {
            if ( StateIndex < 0 || StateIndex >= m_SAnimationDatas.Length )
            {
                CLOG.E ( "StateIndex error!! can not play!" );
                return;
            }

            //如果在播放就停止
            if ( m_IsPlaying )
            {
                StopNowAnimation();
            }

            m_IsLoop = IsLoop;
            m_NowPlayerStateIndex = StateIndex;
            m_PlayHeadIndex = 0;

            var stateAnim = m_SAnimationDatas[m_NowPlayerStateIndex].StateAnimation;

            int AnimCount = stateAnim.Frames.Length;
            if ( AnimCount > 1 )
            {
                m_PlayCoroutine = StartCoroutine ( PlayHandler ( stateAnim ) );
            }
            else if ( AnimCount == 1 )
            {
                SetSprite ( stateAnim.Frames[0].SpFrame );
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
        public void SetEveryFrameCallBack ( string StateName,  DelegateAnimationFrameEvent callback )
        {
            SetEveryFrameCallBack ( GetStateIndexByName ( StateName ), callback );
        }

        /// <summary>
        /// 设置特定动画每一帧的回调
        /// </summary>
        public void SetEveryFrameCallBack ( int StateIndex, DelegateAnimationFrameEvent callback )
        {
            m_SAnimationDatas[StateIndex].StateAnimation.SetEveryFrameCallBack ( callback );
        }

        /// <summary>
        /// 添加特定动画特定帧回调
        /// </summary>
        public void AddFrameCallBack ( string StateName, int FrameIndex, DelegateAnimationFrameEvent callback )
        {
            AddFrameCallBack ( GetStateIndexByName ( StateName ), FrameIndex, callback );
        }

        /// <summary>
        /// 设置特定动画每一帧的回调
        /// </summary>
        public void AddFrameCallBack ( int StateIndex, int FrameIndex, DelegateAnimationFrameEvent callback )
        {
            m_SAnimationDatas[StateIndex].StateAnimation.AddFrameCallBack ( FrameIndex, callback );
        }

        /// <summary>
        /// 移除特定动画特定帧的回调
        /// </summary>
        public void DeleteFrameCallBack ( string StateName, int FrameIndex )
        {
            DeleteFrameCallBack ( GetStateIndexByName ( StateName ), FrameIndex );
        }


        /// <summary>
        /// 移除特定动画特定帧的回调
        /// </summary>
        public void DeleteFrameCallBack ( int StateIndex, int FrameIndex )
        {
            m_SAnimationDatas[StateIndex].StateAnimation.DeleteFrameCallBack ( FrameIndex );
        }

        /// <summary>
        /// 移除特定动画所有帧回掉
        /// </summary>
        public void DeleteAllFrameCallBack ( string StateName  )
        {
            DeleteAllFrameCallBack ( GetStateIndexByName ( StateName ) );
        }


        /// <summary>
        /// 移除特定动画所有帧回掉
        /// </summary>
        public void DeleteAllFrameCallBack ( int StateIndex )
        {
            m_SAnimationDatas[StateIndex].StateAnimation.DeleteAllFrameCallBack();
        }

        /// <summary>
        /// 通过状态名得到状态序号
        /// </summary>
        /// <param name="StateName"></param>
        /// <returns></returns>
        private int GetStateIndexByName ( string StateName )
        {
            for ( int i = 0; i < m_SAnimationDatas.Length; i++ )
            {
                if ( m_SAnimationDatas[i].StateName == StateName )
                {
                    return i;
                }
            }

            return -1;
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
            if ( m_SAnimationDatas != null &&
                    m_SAnimationDatas.Length != 0 &&
                    m_SAnimationDatas[0].StateAnimation != null &&
                    m_SAnimationDatas[0].StateAnimation.Frames != null &&
                    m_SAnimationDatas[0].StateAnimation.Frames.Length != 0 )
            {
                SetSprite ( m_SAnimationDatas[0].StateAnimation.Frames[0].SpFrame );
            }
        }
#endif

    }


}