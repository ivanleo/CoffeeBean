﻿using UnityEngine;
using DG.Tweening;
using UnityEngine.UI;
using UnityEngine.EventSystems;
using System;
using System.Collections.Generic;
using System.Reflection;
using System.Text;

namespace CoffeeBean
{
    /// <summary>
    /// 无限滚动
    /// </summary>
    [RequireComponent ( typeof ( ScrollRect ) )]
    public class CInfiniteScroll : MonoBehaviour
    {
        /// <summary>
        /// 滚动方向
        /// </summary>
        [SerializeField]
        private EPageDirection _PageDir;

        /// <summary>
        /// 是否循环滚动
        /// </summary>
        [SerializeField]
        private bool _IsLoop = false;

        /// <summary>
        /// 子项数
        /// </summary>
        public int ChildCount { get; private set; }

        /// <summary>
        /// 子项尺寸
        /// </summary>
        private Vector2 _ChildSize;

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
        /// 数据容器
        /// </summary>
        private List<IRepScrollItemData> _Data;

        /// <summary>
        /// 数据队列
        /// </summary>
        private LinkedList<int> _DataIndexes;

        /// <summary>
        /// 苏醒时
        /// </summary>
        private void Awake()
        {
            _sr = GetComponent<ScrollRect>();
            if ( _sr.horizontalScrollbar != null ) { _sr.horizontalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide; }
            if ( _sr.verticalScrollbar != null ) { _sr.verticalScrollbarVisibility = ScrollRect.ScrollbarVisibility.AutoHide; }

            _content = _sr.content;
            _viewSize = _sr.viewport.rect.size;

            _sr.onValueChanged.AddListener ( OnScroll );

            _DataIndexes = new LinkedList<int>();
        }

        /// <summary>
        /// 滚动中
        /// </summary>
        /// <param name="arg0"></param>
        private void OnScroll ( Vector2 pos )
        {
            switch ( _PageDir )
            {
                case EPageDirection.LEFT_TO_RIGHT:
                    if ( _sr.content.anchoredPosition.x < - _ChildSize.x * 2 && MoveFirstToLast() )
                    {
                        _sr.content.anchoredPosition += new Vector2 ( _ChildSize.x, 0  );
                    }
                    else if ( _sr.content.anchoredPosition.x > -_ChildSize.x && MoveLastToFirst() )
                    {
                        _sr.content.anchoredPosition -= new Vector2 ( _ChildSize.x, 0 );
                    }

                    break;
                case EPageDirection.UP_TO_DOWN:
                    if ( _sr.content.anchoredPosition.y > _ChildSize.y * 2 && MoveFirstToLast() )
                    {
                        _sr.content.anchoredPosition -= new Vector2 ( 0f, _ChildSize.y );
                    }
                    else if ( _sr.content.anchoredPosition.y < _ChildSize.y && MoveLastToFirst() )
                    {
                        _sr.content.anchoredPosition += new Vector2 ( 0f, _ChildSize.y );
                    }
                    break;
            }
        }

        /// <summary>
        /// 得到当前显示的数据下标
        /// </summary>
        /// <returns></returns>
        public int[] GetNowShowIndex()
        {
            int[] indexes = new int[_DataIndexes.Count];
            _DataIndexes.CopyTo ( indexes, 0 );
            return indexes;
        }

        /// <summary>
        /// 把第一项移动到尾巴
        /// </summary>
        private bool MoveFirstToLast()
        {
            if ( _DataIndexes.Last.Value >= _Data.Count - 1 && !_IsLoop )
            {
                return false;
            }

            _DataIndexes.RemoveFirst();

            Transform node = _sr.content.GetChild ( 0 );
            node.SetSiblingIndex ( _sr.content.childCount - 1 );

            int dataindex = MakeIndexInRightArea ( _DataIndexes.Last.Value + 1 );
            node.GetComponent<IRepScrollItem>().FillData ( _Data[dataindex] );

            _DataIndexes.AddLast ( dataindex );

            return true;
        }

        /// <summary>
        /// 把尾巴移动到第一项
        /// </summary>
        private bool MoveLastToFirst()
        {
            if ( _DataIndexes.First.Value < 1 && !_IsLoop )
            {
                return false;
            }

            _DataIndexes.RemoveLast();

            Transform node = _sr.content.GetChild ( _sr.content.childCount - 1 );
            node.SetSiblingIndex ( 0 );

            int dataindex = MakeIndexInRightArea ( _DataIndexes.First.Value - 1 );
            node.GetComponent<IRepScrollItem>().FillData ( _Data[dataindex] );
            _DataIndexes.AddFirst ( dataindex );

            return true;
        }

        /// <summary>
        /// 使序号处于正确的区间
        /// </summary>
        /// <param name="index"></param>
        /// <returns></returns>
        private int MakeIndexInRightArea ( int index )
        {
            while ( index < 0 )
            {
                index += _Data.Count;
            }

            while ( index >= _Data.Count )
            {
                index -= _Data.Count;
            }
            return index;
        }


        /// <summary>
        /// 绑定数据
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="itemCount"></param>
        /// <param name="data"></param>
        /// <param name="itemtp"></param>
        public void Bind ( List< IRepScrollItemData > data, Type itemtp )
        {
            _Data = data;
            int nowCreateCount = 0;

            //至少创建1个
            int itemCount = 1;
            do
            {
                //创建子节点
                object ob = CFunction.CallStaticFunction ( "CreateUI", itemtp.BaseType );
                ( ob as MonoBehaviour ).transform.SetParent ( _content, false );
                ( ob as IRepScrollItem ).FillData ( data[nowCreateCount] );

                //记录当前显示的数据项序号
                _DataIndexes.AddLast ( nowCreateCount );

                nowCreateCount++;

                //如果需要自动计算子项数
                //那么在创建完第一个后就开启计算
                if ( nowCreateCount == 1 )
                {
                    RectTransform rt = ( ob as MonoBehaviour ).transform as RectTransform;
                    itemCount = ComputeChildCount ( rt.sizeDelta );
                }
            }
            while ( nowCreateCount < itemCount );

            //记录子项数
            ChildCount = itemCount;
        }

        /// <summary>
        /// 计算子项数
        /// </summary>
        /// <param name="ChildSize"></param>
        /// <returns></returns>
        private int ComputeChildCount ( Vector2 ChildSize )
        {
            _ChildSize = ChildSize;

            if ( _PageDir == EPageDirection.LEFT_TO_RIGHT )
            {
                return Mathf.CeilToInt ( _viewSize.x / ChildSize.x ) + 2;
            }
            else if ( _PageDir == EPageDirection.UP_TO_DOWN )
            {
                return Mathf.CeilToInt ( _viewSize.y / ChildSize.y ) + 2;
            }

            return -1;
        }


    }

}
