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
    public delegate void DelegatePageEvent ( int oldPageIndex, int newPageIndex );

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
        private EPageDirection _PageDir;

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
        private int _nowIndex = 0;

        /// <summary>
        /// 当前显示的页码
        /// </summary>
        public int NowIndex {  get { return _nowIndex; } }

        /// <summary>
        /// 滚动事件
        /// </summary>
        public event DelegatePageEvent OnPageChange;

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
                    ScrollTo ( _nowIndex - offIndex );
                }
                else if ( _PageDir == EPageDirection.UP_TO_DOWN )
                {
                    ScrollTo ( _nowIndex + offIndex );
                }
            }
            else
            {
                ScrollTo ( _nowIndex );
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

            int temp = _nowIndex;
            _nowIndex = targetindex;

            Tweener tw = null;
            if ( _PageDir == EPageDirection.UP_TO_DOWN )
            {
                tw = _sr.DOVerticalNormalizedPos ( 1 - ( float ) _nowIndex / ( _content.childCount - 1 ), costTime );
            }
            else if ( _PageDir == EPageDirection.LEFT_TO_RIGHT )
            {
                tw = _sr.DOHorizontalNormalizedPos ( ( float ) _nowIndex / ( _content.childCount - 1 ), costTime );
            }

            tw.onComplete = () =>
            {
                if ( OnPageChange != null )
                {
                    OnPageChange ( temp, targetindex );
                }
            };
        }

        /// <summary>
        /// 向前移动一页
        /// </summary>
        public void ScrollPrev()
        {
            if ( _nowIndex < 1 )
            {
                return;
            }

            ScrollTo ( _nowIndex - 1 );
        }

        /// <summary>
        /// 向后移动一页
        /// </summary>
        public void ScrollNext()
        {
            if ( _nowIndex > _content.childCount - 2 )
            {
                return;
            }

            ScrollTo ( _nowIndex + 1 );
        }


    }

}
