/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/13 09:18
	File base:	CPageView.cs
	author:		Leo

	purpose:	PageView组件
                实现分页浏览滚动效果
*********************************************************************/
using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;

namespace CoffeeBean
{
    /// <summary>
    /// Pageview事件
    /// </summary>
    /// <param name="oldPageIndex"></param>
    /// <param name="newPageIndex"></param>
    public delegate void DGPageEvent ( int oldPageIndex, int newPageIndex );

    /// <summary>
    /// 页面方向
    /// </summary>
    public enum EPageDirection
    {
        /// <summary>
        /// 上到下
        /// </summary>
        UP_TO_DOWN,
        /// <summary>
        /// 左到右
        /// </summary>
        LEFT_TO_RIGHT
    }

    /// <summary>
    /// 页面滚动
    /// </summary>
    [RequireComponent ( typeof ( ScrollRect ) )]
    public class CPageView: MonoBehaviour, IEndDragHandler, IBeginDragHandler
    {
        /// <summary>
        /// 滚动方向
        /// </summary>
        [SerializeField]
        private EPageDirection _PageDir = EPageDirection.LEFT_TO_RIGHT;

        /// <summary>
        /// 拖动多少就自动翻页
        /// </summary>
        [Range ( 0f, 1f )]
        [SerializeField]
        private float _StartRatio = 0.33f;

        /// <summary>
        /// 滚动矩形
        /// </summary>
        private ScrollRect _sr;

        /// <summary>
        /// 内容区域
        /// </summary>
        private RectTransform _content;

        /// <summary>
        /// 视口尺寸
        /// </summary>
        private Vector2 _viewSize;

        /// <summary>
        /// 当前显示的页码
        /// </summary>
        public int NowIndex { get; private set; } = 0;

        /// <summary>
        /// 滚动事件
        /// </summary>
        public event DGPageEvent OnPageChange;

        /// <summary>
        /// 开始拖拽的坐标
        /// </summary>
        private Vector2 _StartDragPos;

        /// <summary>
        /// 苏醒时
        /// </summary>
        private void Awake()
        {
            _sr = GetComponent<ScrollRect>();
            _content = _sr.content;
            _viewSize = _sr.viewport.rect.size;

            //取消惯性
            _sr.inertia = false;

            //取消hui'tan
            _sr.movementType = ScrollRect.MovementType.Clamped;
        }

        /// <summary>
        /// 开始拖拽
        /// </summary>
        /// <param name="eventData"></param>
        public void OnBeginDrag ( PointerEventData eventData )
        {
            _StartDragPos = _sr.content.anchoredPosition;
        }

        /// <summary>
        /// 停止拖拽时
        /// </summary>
        /// <param name="eventData"></param>
        public void OnEndDrag ( PointerEventData eventData )
        {
            Vector2 offset = _sr.content.anchoredPosition - _StartDragPos;
            float offratio = 0f;

            if ( _PageDir == EPageDirection.LEFT_TO_RIGHT )
            {
                offratio = offset.x / _viewSize.x;
            }
            else if ( _PageDir == EPageDirection.UP_TO_DOWN )
            {
                offratio = offset.y / _viewSize.y;
            }

            if ( Mathf.Abs ( offratio ) > _StartRatio )
            {
                int offIndex = offratio > 0 ? Mathf.CeilToInt ( offratio ) : Mathf.FloorToInt ( offratio );

                if ( _PageDir == EPageDirection.LEFT_TO_RIGHT )
                {
                    ScrollTo ( NowIndex - offIndex );
                }
                else if ( _PageDir == EPageDirection.UP_TO_DOWN )
                {
                    ScrollTo ( NowIndex + offIndex );
                }
            }
            else
            {
                ScrollTo ( NowIndex );
            }
        }

        /// <summary>
        /// 跳转到指定页面
        /// </summary>
        /// <param name="targetindex">目标页面序号 0开始 </param>
        /// <param name="costTime">花费时间</param>
        public void ScrollTo ( int targetindex, float costTime = 0.5f )
        {
            if ( targetindex < 0 || targetindex >= _content.childCount )
            {
                return;
            }

            int temp = NowIndex;
            NowIndex = targetindex;

            Tweener tw = null;

            if ( _PageDir == EPageDirection.UP_TO_DOWN )
            {
                tw = _sr.DOVerticalNormalizedPos ( 1 - ( float ) NowIndex / ( _content.childCount - 1 ), costTime );
            }
            else if ( _PageDir == EPageDirection.LEFT_TO_RIGHT )
            {
                tw = _sr.DOHorizontalNormalizedPos ( ( float ) NowIndex / ( _content.childCount - 1 ), costTime );
            }

            tw.onComplete = () =>
            {
                OnPageChange?.Invoke ( temp, targetindex );
            };
        }

        /// <summary>
        /// 向前移动一页
        /// </summary>
        public void ScrollPrev()
        {
            if ( NowIndex < 1 )
            {
                return;
            }

            ScrollTo ( NowIndex - 1 );
        }

        /// <summary>
        /// 向后移动一页
        /// </summary>
        public void ScrollNext()
        {
            if ( NowIndex > _content.childCount - 2 )
            {
                return;
            }

            ScrollTo ( NowIndex + 1 );
        }


    }

}
