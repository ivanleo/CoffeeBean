/********************************************************************
	created:	2018/08/17
	created:	17:8:2018   12:31
	filename: 	D:\Work\CoffeeBean\Assets\CoffeeBean\Animation\CFrameAnimationBase.cs
	file path:	D:\Work\CoffeeBean\Assets\CoffeeBean\Animation
	file base:	CFrameAnimationBase
	file ext:	cs
	author:		Leo

	purpose:	序列帧动画基类
*********************************************************************/
using System.Collections;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 序列帧动画基类
    /// </summary>
    public class CFrameAnimationBase : MonoBehaviour
    {
        /// <summary>
        /// 是否在开始时默认播放第一个动画
        /// </summary>
        [SerializeField]
        protected bool m_AutoPlayOnStart = false;

        /// <summary>
        /// 当前是否重复播放
        /// </summary>
        [SerializeField]
        protected bool m_IsLoop = false;

        /// <summary>
        /// 是否正在播放动画
        /// </summary>
        [ReadOnly]
        [SerializeField]
        protected bool m_IsPlaying = false;

        /// <summary>
        /// 当前播放头
        /// </summary>
        [ReadOnly]
        [SerializeField]
        protected int m_PlayHeadIndex = 0;

        /// <summary>
        /// 操控的图像 支持 SpriteRenderer 和 Image两种
        /// </summary>
        protected SpriteRenderer m_SR = null;
        protected Image m_Img = null;

        /// <summary>
        /// 播放协程
        /// </summary>
        protected Coroutine m_PlayCoroutine = null;

        /// <summary>
        /// 得到播放头
        /// </summary>
        /// <returns></returns>
        public int GetPlayerHeadIndex()
        {
            return m_PlayHeadIndex;
        }

        /// <summary>
        /// 苏醒时
        /// </summary>
        protected void Awake()
        {
            m_SR = GetComponent<SpriteRenderer>();
            m_Img = GetComponent<Image>();

            if ( m_SR == null && m_Img == null )
            {
                CLOG.E ( "{0} has no Image or SpriteRender Component! so the frame animation can not work!" );
                return;
            }
        }

        /// <summary>
        /// 设置精灵
        /// </summary>
        /// <param name="Sp"></param>
        protected void SetSprite ( Sprite Sp )
        {
            if ( m_SR != null )
            {
                m_SR.sprite = Sp;
            }
            else if ( m_Img != null )
            {
                m_Img.sprite = Sp;
            }
            else
            {
                CLOG.E ( "{0} has no Image or SpriteRender Component! so the frame animation can not work!", gameObject.name );
            }
        }

        /// <summary>
        /// 播放处理
        /// </summary>
        /// <returns>协程</returns>
        protected IEnumerator PlayHandler ( CAnimation Anim )
        {
            while ( true )
            {
                if ( m_PlayHeadIndex < 0 || m_PlayHeadIndex >= Anim.Frames.Length )
                {
                    CLOG.E ( "frame index error!!" );
                    break;
                }

                SetSprite ( Anim.Frames[m_PlayHeadIndex].SpFrame );

                /************************************************************************/
                /*                              回调处理                                */
                /************************************************************************/
                if ( Anim.FrameCallbacks.ContainsKey ( m_PlayHeadIndex ) )
                {
                    //如果特定帧回调存在，那么调用它
                    if ( Anim.FrameCallbacks[m_PlayHeadIndex] != null )
                    {
                        Anim.FrameCallbacks[m_PlayHeadIndex] ( m_PlayHeadIndex );
                    }
                }
                else if ( Anim.EveryFrameCallback != null )
                {
                    //如果特定帧回调不存在，而帧回调存在，那么调用帧回调
                    Anim.EveryFrameCallback ( m_PlayHeadIndex );
                }

                //下一帧
                m_PlayHeadIndex++;

                //安全边界处理
                if ( m_PlayHeadIndex >= Anim.Frames.Length )
                {
                    if ( m_IsLoop )
                    {
                        m_PlayHeadIndex = 0;
                    }
                    else
                    {
                        break;
                    }
                }

                yield return new WaitForSeconds ( Anim.Frames[m_PlayHeadIndex].SpInteval );
            }

            m_PlayCoroutine = null;
            m_IsPlaying = false;
        }

        /// <summary>
        /// 停止当前动画的播放
        /// </summary>
        /// <param name="SetFrameToZero">是否回到第一帧</param>
        public void StopNowAnimation ( Sprite StopSP = null )
        {
            if ( !m_IsPlaying )
            {
                return;
            }

            if ( m_PlayCoroutine != null )
            {
                StopCoroutine ( m_PlayCoroutine );
                m_PlayCoroutine = null;
            }

            //设置回到第一帧
            if ( StopSP != null )
            {
                SetSprite ( StopSP );
            }

            m_IsPlaying = false;
        }
    }

}

