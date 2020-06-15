/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/13 23:27
	File base:	CUIAnimtaionHandler.cs
	author:		Leo

	purpose:	UI动画处理器
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using DG.Tweening;

namespace CoffeeBean
{
    /// <summary>
    /// UI进场动画枚举
    /// </summary>
    [Flags]
    public enum EAnim_In : byte
    {
        [CEnumDesc ( "无动画" )]
        NONE = 1,

        [CEnumDesc ( "淡入" )]
        FADE_IN = 2,

        [CEnumDesc ( "放大进入" )]
        SCALE_BIG = 4,

        [CEnumDesc ( "缩小进入" )]
        SCALE_SMALL = 8,

        [CEnumDesc ( "从左边进入" )]
        FROM_LEFT_IN = 16,

        [CEnumDesc ( "从右边进入" )]
        FROM_RIGHT_IN = 32,

        [CEnumDesc ( "从上面进入" )]
        FROM_UP_IN = 64,

        [CEnumDesc ( "从下面进入" )]
        FROM_DOWN_IN = 128,
    }

    /// <summary>
    /// 退场动画枚举
    /// </summary>
    [Flags]
    public enum EAnim_Out : byte
    {
        [CEnumDesc ( "无动画" )]
        NONE = 1,

        [CEnumDesc ( "淡出" )]
        FADE_OUT = 2,

        [CEnumDesc ( "放大退出" )]
        SCALE_BIG = 4,

        [CEnumDesc ( "缩小退出" )]
        SCALE_SMALL = 8,

        [CEnumDesc ( "到左边退出" )]
        TO_LEFT_OUT = 16,

        [CEnumDesc ( "到右边退出" )]
        TO_RIGHT_OUT = 32,

        [CEnumDesc ( "到上面退出" )]
        TO_UP_OUT = 64,

        [CEnumDesc ( "到下面退出" )]
        TO_DOWN_OUT = 128,
    }

    /// <summary>
    /// UI动画处理器
    /// </summary>
    public static class CUIAnimtaion
    {
        /// <summary>
        /// 通用 入场动画，放大，并且淡入
        /// 执行 await UI_Test.CreateUIWithAnim(CUIAnimtaion.AnimIn_ScaleBigAndFadeIn);
        /// 即可看到效果
        /// </summary>
        public static readonly CUIAnimIn AnimIn_ScaleBigAndFadeIn = new CUIAnimIn(){Anim=EAnim_In.FADE_IN|EAnim_In.SCALE_BIG,time = 0.5f,ease = Ease.OutBack};

        /// <summary>
        /// 通用 退场动画
        /// 执行 await UI_Test.DestroyUIWithAnim(CUIAnimtaion.AnimOut_ScaleSmallAndFadeOut);
        /// 即可看到效果
        /// </summary>
        public static readonly CUIAnimOut AnimOut_ScaleSmallAndFadeOut = new CUIAnimOut(){Anim=EAnim_Out.FADE_OUT|EAnim_Out.SCALE_SMALL,time = 0.5f,ease = Ease.InOutElastic};

        /// <summary>
        /// 执行入场动画
        /// </summary>
        /// <param name="TargetUI">目标UI</param>
        /// <param name="AnimInInfo">进入动画信息</param>
        public static async Task PlayInAnim( RectTransform TargetUI, CUIAnimIn AnimInInfo )
        {
            var anim = AnimInInfo.Anim;
            var time = AnimInInfo.time;
            var ease = AnimInInfo.ease;

            bool hasAnim = false;

            // 淡入
            if ( AnimInInfo.Anim.HasFlag( EAnim_In.FADE_IN ) )
            {
                hasAnim = true;
                TargetUI.FadeInUINode( time );
            }

            // 放大进入
            if ( anim.HasFlag( EAnim_In.SCALE_BIG ) )
            {
                hasAnim = true;
                TargetUI.localScale = Vector3.zero;
                TargetUI.DOScale( Vector3.one, time ).SetEase( ease );
            }

            // 缩小进入
            if ( anim.HasFlag( EAnim_In.SCALE_SMALL ) )
            {
                hasAnim = true;
                TargetUI.localScale = Vector3.one * 3;
                TargetUI.DOScale( Vector3.one, time ).SetEase( ease );
            }

            // 从左边进入
            if ( anim.HasFlag( EAnim_In.FROM_LEFT_IN ) )
            {
                hasAnim = true;
                var nowPos = TargetUI.anchoredPosition;
                TargetUI.anchoredPosition = new Vector2( nowPos.x - Screen.width, nowPos.y );
                TargetUI.DOAnchorPosX( Screen.width, time ).SetRelative( true ).SetEase( ease );
            }

            // 从右边进入
            if ( anim.HasFlag( EAnim_In.FROM_RIGHT_IN ) )
            {
                hasAnim = true;
                var nowPos = TargetUI.anchoredPosition;
                TargetUI.anchoredPosition = new Vector2( nowPos.x + Screen.width, nowPos.y );
                TargetUI.DOAnchorPosX( -Screen.width, time ).SetRelative( true ).SetEase( ease );
            }

            // 从上面进入
            if ( anim.HasFlag( EAnim_In.FROM_UP_IN ) )
            {
                hasAnim = true;
                var nowPos = TargetUI.anchoredPosition;
                TargetUI.anchoredPosition = new Vector2( nowPos.x, nowPos.y + Screen.height );
                TargetUI.DOAnchorPosY( -Screen.height, time ).SetRelative( true ).SetEase( ease );
            }

            // 从下面进入
            if ( anim.HasFlag( EAnim_In.FROM_DOWN_IN ) )
            {
                hasAnim = true;
                var nowPos = TargetUI.anchoredPosition;
                TargetUI.anchoredPosition = new Vector2( nowPos.x, nowPos.y - Screen.height );
                TargetUI.DOAnchorPosY( Screen.height, time ).SetRelative( true ).SetEase( ease );
            }

            if ( hasAnim )
            {
                await new WaitForSeconds( time );
            }
        }

        /// <summary>
        /// 执行退场动画
        /// </summary>
        /// <param name="TargetUI">目标UI</param>
        /// <param name="AnimOutInfo">退出动画信息</param>
        public static async Task PlayOutAnim( RectTransform TargetUI, CUIAnimOut AnimOutInfo )
        {
            var anim = AnimOutInfo.Anim;
            var time = AnimOutInfo.time;
            var ease = AnimOutInfo.ease;

            bool hasAnim = false;

            // 淡出
            if ( anim.HasFlag( EAnim_Out.FADE_OUT ) )
            {
                hasAnim = true;
                TargetUI.FadeOutUINode( time );
            }

            // 放大退出
            if ( anim.HasFlag( EAnim_Out.SCALE_BIG ) )
            {
                hasAnim = true;
                TargetUI.DOScale( Vector3.one * 3, time ).SetEase( ease );
            }

            // 缩小退出
            if ( anim.HasFlag( EAnim_Out.SCALE_SMALL ) )
            {
                hasAnim = true;
                TargetUI.DOScale( Vector3.zero, time ).SetEase( ease );
            }

            // 到左边退出
            if ( anim.HasFlag( EAnim_Out.TO_LEFT_OUT ) )
            {
                hasAnim = true;
                TargetUI.DOAnchorPosX( -Screen.width, time ).SetRelative( true ).SetEase( ease );
            }

            // 到右边退出
            if ( anim.HasFlag( EAnim_Out.TO_RIGHT_OUT ) )
            {
                hasAnim = true;
                TargetUI.DOAnchorPosX( Screen.width, time ).SetRelative( true ).SetEase( ease );
            }

            // 到上面退出
            if ( anim.HasFlag( EAnim_Out.TO_UP_OUT ) )
            {
                hasAnim = true;
                TargetUI.DOAnchorPosY( Screen.height, time ).SetRelative( true ).SetEase( ease );
            }

            // 到下面退出
            if ( anim.HasFlag( EAnim_Out.TO_DOWN_OUT ) )
            {
                hasAnim = true;
                TargetUI.DOAnchorPosY( -Screen.height, time ).SetRelative( true ).SetEase( ease );
            }

            if ( hasAnim )
            {
                await new WaitForSeconds( time );
            }
        }
    }

    /// <summary>
    /// 呈现动画信息
    /// </summary>
    public class CUIAnimIn
    {
        /// <summary>
        /// 入场动画枚举
        /// </summary>
        public EAnim_In Anim;

        /// <summary>
        /// 缓动函数
        /// </summary>
        public Ease ease;

        /// <summary>
        /// 动画时间
        /// </summary>
        public float time;
    }

    /// <summary>
    /// 退出动画信息
    /// </summary>
    public class CUIAnimOut
    {
        /// <summary>
        /// 退场动画枚举
        /// </summary>
        public EAnim_Out Anim;

        /// <summary>
        /// 缓动函数
        /// </summary>
        public Ease ease;

        /// <summary>
        /// 动画时间
        /// </summary>
        public float time;
    }
}