/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07
	Filee: 	    CInfiniteScroll.cs
	Author:		Leo

	Purpose:	无限滚动组件
                滚动区永远只有特定数目的元素
                通过将移出屏幕的元素移动到滚动区尾部来实现无限滚动
*********************************************************************/

using System;
using System.Collections.Generic;
using DG.Tweening;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// 滚动事件
    /// </summary>
    public enum EScrollEvent
    {
        /// <summary>
        /// 待机
        /// </summary>
        IDLE,

        /// <summary>
        /// 滚动中
        /// </summary>
        SCROLLING,

        /// <summary>
        /// 停止了，速度为0
        /// </summary>
        STOP,

        /// <summary>
        /// 修复位置中
        /// </summary>
        FIXED_POS,
    }

    /// <summary>
    /// 可重复利用的滚动项 数据接口
    /// </summary>
    public interface IInfScrollItemData
    {
    }

    /// <summary>
    /// 可重复利用的滚动项 UI接口
    /// </summary>
    public interface IInfScrollItemUI
    {
        /// <summary>
        /// 填充数据
        /// </summary>
        /// <param name="data"></param>
        void FillData( IInfScrollItemData data );
    }

    /// <summary>
    /// 无限滚动
    /// </summary>
    [RequireComponent( typeof( ScrollRect ) )]
    public class CInfiniteScroll : MonoBehaviour
    {
        /// <summary>
        /// 子项尺寸
        /// </summary>
        private Vector2 m_ChildSize;

        /// <summary>
        /// 内容区域
        /// </summary>
        private RectTransform m_Content;

        /// <summary>
        /// 数据容器
        /// </summary>
        private List<IInfScrollItemData> m_Data;

        /// <summary>
        /// 数据队列
        /// </summary>
        private LinkedList<int> m_DataIndexes;

        /// <summary>
        /// 是否循环滚动
        /// </summary>
        [SerializeField]
        private bool m_IsLoop = false;

        /// <summary>
        /// 滚动方向
        /// </summary>
        [SerializeField]
        private EPageDirection m_PageDir = EPageDirection.UP_TO_DOWN;

        /// <summary>
        /// 滚动状态
        /// </summary>
        [SerializeField]
        [CReadOnly]
        private EScrollEvent m_ScorllEvent = EScrollEvent.IDLE;

        /// <summary>
        /// 滚动矩形
        /// </summary>
        private ScrollRect m_SR;

        /// <summary>
        /// 步骤滚动
        /// 若停止时没有位于单个选项得整数倍
        /// 则自动滚回去
        /// </summary>
        [SerializeField]
        private bool m_StepScroll = false;

        /// <summary>
        /// 视口尺寸
        /// </summary>
        private Vector2 m_ViewSize;

        /// <summary>
        /// 子项数
        /// </summary>
        public int ChildCount { get; private set; }

        /// <summary>
        /// 滚动状态
        /// </summary>
        public EScrollEvent ScorllEvent { get { return m_ScorllEvent; } }

        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemCount"></param>
        /// <param name="data"></param>
        /// <param name="itemtp"></param>
        public void Bind( List<IInfScrollItemData> data, Type itemtp )
        {
            m_Data = data;
            int nowCreateCount = 0;

            //至少创建1个
            int itemCount = 1;

            do
            {
                //创建子节点
                object ob = CReflect.CallStaticFunction (  itemtp.BaseType, "CreateUI", m_Content );
                ( ob as IInfScrollItemUI ).FillData( data[nowCreateCount] );

                //记录当前显示的数据项序号
                m_DataIndexes.AddLast( nowCreateCount );

                nowCreateCount++;

                //如果需要自动计算子项数
                //那么在创建完第一个后就开启计算
                if ( nowCreateCount == 1 )
                {
                    RectTransform rt = ( ob as MonoBehaviour ).transform as RectTransform;
                    itemCount = ComputeChildCount( rt.sizeDelta );
                }
            } while ( nowCreateCount < itemCount );

            //记录子项数
            ChildCount = itemCount;
        }

        /// <summary>
        /// 得到当前显示的数据下标
        /// </summary>
        /// <returns></returns>
        public int[] GetNowShowIndex()
        {
            int [ ] indexes = new int [m_DataIndexes.Count];
            m_DataIndexes.CopyTo( indexes, 0 );
            return indexes;
        }

        /// <summary>
        /// 苏醒时
        /// </summary>
        private void Awake()
        {
            m_SR = GetComponent<ScrollRect>();

            if ( m_SR.horizontalScrollbar != null )
            {
                m_SR.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            }

            if ( m_SR.verticalScrollbar != null )
            {
                m_SR.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide;
            }

            m_Content = m_SR.content;
            m_ViewSize = m_SR.viewport.rect.size;

            m_SR.onValueChanged.AddListener( OnScroll );

            m_DataIndexes = new LinkedList<int>();
        }

        /// <summary>
        /// 计算子项数
        /// </summary>
        /// <param name="ChildSize"></param>
        /// <returns></returns>
        private int ComputeChildCount( Vector2 ChildSize )
        {
            m_ChildSize = ChildSize;

            if ( m_PageDir == EPageDirection.LEFT_TO_RIGHT )
            {
                return Mathf.CeilToInt( m_ViewSize.x / ChildSize.x ) + 2;
            }
            else if ( m_PageDir == EPageDirection.UP_TO_DOWN )
            {
                return Mathf.CeilToInt( m_ViewSize.y / ChildSize.y ) + 2;
            }

            return -1;
        }

        /// <summary>
        /// 使序号处于正确的区间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int MakeIndexInRightArea( int index )
        {
            while ( index < 0 )
            {
                index += m_Data.Count;
            }

            while ( index >= m_Data.Count )
            {
                index -= m_Data.Count;
            }

            return index;
        }

        /// <summary>
        /// 把第一项移动到尾巴
        /// </summary>
        private bool MoveFirstToLast()
        {
            if ( m_DataIndexes.Last.Value >= m_Data.Count - 1 && !m_IsLoop )
            {
                return false;
            }

            m_DataIndexes.RemoveFirst();

            Transform node = m_SR.content.GetChild ( 0 );
            node.SetSiblingIndex( m_SR.content.childCount - 1 );

            int dataindex = MakeIndexInRightArea ( m_DataIndexes.Last.Value + 1 );
            node.GetComponent<IInfScrollItemUI>().FillData( m_Data[dataindex] );

            m_DataIndexes.AddLast( dataindex );

            return true;
        }

        /// <summary>
        /// 把尾巴移动到第一项
        /// </summary>
        private bool MoveLastToFirst()
        {
            if ( m_DataIndexes.First.Value < 1 && !m_IsLoop )
            {
                return false;
            }

            m_DataIndexes.RemoveLast();

            Transform node = m_SR.content.GetChild ( m_SR.content.childCount - 1 );
            node.SetSiblingIndex( 0 );

            int dataindex = MakeIndexInRightArea ( m_DataIndexes.First.Value - 1 );
            node.GetComponent<IInfScrollItemUI>().FillData( m_Data[dataindex] );
            m_DataIndexes.AddFirst( dataindex );

            return true;
        }

        /// <summary>
        /// 移动到最近得项
        /// </summary>
        private void MoveToNearestItem()
        {
            switch ( m_PageDir )
            {
            case EPageDirection.LEFT_TO_RIGHT:
                int index_x = Mathf.RoundToInt ( m_SR.content.anchoredPosition.x / m_ChildSize.x );
                m_SR.content.DOAnchorPosY( index_x * m_ChildSize.x, 1f );
                break;

            case EPageDirection.UP_TO_DOWN:
                int index_y = Mathf.RoundToInt ( m_SR.content.anchoredPosition.y / m_ChildSize.y );
                m_SR.content.DOAnchorPosY( index_y * m_ChildSize.y, 1f );
                break;
            }
        }

        /// <summary>
        /// 滚动中
        /// </summary>
        /// <param name="arg0"></param>
        private void OnScroll( Vector2 pos )
        {
            if ( ChildCount == 0 )
            {
                return;
            }

            switch ( m_PageDir )
            {
            case EPageDirection.LEFT_TO_RIGHT:
                if ( m_SR.content.anchoredPosition.x < -m_ChildSize.x * 2 && MoveFirstToLast() )
                {
                    m_SR.content.anchoredPosition += new Vector2( m_ChildSize.x, 0 );
                }
                else if ( m_SR.content.anchoredPosition.x > -m_ChildSize.x && MoveLastToFirst() )
                {
                    m_SR.content.anchoredPosition -= new Vector2( m_ChildSize.x, 0 );
                }

                break;

            case EPageDirection.UP_TO_DOWN:
                if ( m_SR.content.anchoredPosition.y > m_ChildSize.y * 2 && MoveFirstToLast() )
                {
                    m_SR.content.anchoredPosition -= new Vector2( 0f, m_ChildSize.y );
                }
                else if ( m_SR.content.anchoredPosition.y < m_ChildSize.y && MoveLastToFirst() )
                {
                    m_SR.content.anchoredPosition += new Vector2( 0f, m_ChildSize.y );
                }

                break;
            }
        }

        /// <summary>
        /// 每帧更新
        /// </summary>
        private void Update()
        {
            switch ( m_ScorllEvent )
            {
            case EScrollEvent.IDLE:
                if ( m_SR.velocity != Vector2.zero )
                {
                    m_ScorllEvent = EScrollEvent.SCROLLING;
                }

                return;

            case EScrollEvent.SCROLLING:
                if ( m_SR.velocity == Vector2.zero )
                {
                    m_ScorllEvent = EScrollEvent.STOP;
                }

                return;

            case EScrollEvent.STOP:
                if ( m_StepScroll )
                {
                    m_ScorllEvent = EScrollEvent.FIXED_POS;
                    MoveToNearestItem();
                }
                else
                {
                    m_ScorllEvent = EScrollEvent.IDLE;
                }

                return;

            case EScrollEvent.FIXED_POS:
                if ( m_SR.velocity == Vector2.zero )
                {
                    m_ScorllEvent = EScrollEvent.IDLE;
                }

                return;
            }
        }
    }
}