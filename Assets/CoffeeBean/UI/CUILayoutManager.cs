﻿using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// UI层级管理器
    /// </summary>
    public static class CUILayoutManager
    {
        /// <summary>
        /// UI栈
        /// </summary>
        private static Stack<IUIBase> _UIStack = null;

        /// <summary>
        /// 无参构造
        /// </summary>
        static CUILayoutManager()
        {
            _UIStack = new Stack<IUIBase>();
        }

        /// <summary>
        /// 清理所有管理的UI
        /// 一般在场景切换后调用
        /// </summary>
        public static void Clear()
        {
            _UIStack.Clear();
        }


        /// <summary>
        /// 当前栈顶UI隐藏并增加一个新的UI
        /// </summary>
        /// <param name="UI"></param>
        public static void PushUI ( IUIBase UI )
        {
            if ( _UIStack.Count > 0 )
            {
                //当前栈顶UI自动隐藏
                IUIBase Top = _UIStack.Peek();
                if ( Top != null )
                {
                    Top.HideUI();
                }
            }
            UI.IsInLayoutManager = true;
            _UIStack.Push ( UI );
        }

        /// <summary>
        /// 移除栈顶UI并显示新的栈顶UI
        /// </summary>
        /// <param name="UI">要移除的UI</param>
        /// <returns>是否成功移除栈顶，若成功移除，则代表可以删除该UI</returns>
        public static bool PopUI ( IUIBase UI )
        {
            if ( !UI.IsInLayoutManager )
            {
                //CLOG.W ( "the ui {0} is not join the layout manager, so can destroy it", UI.ToString() );
                return true;
            }

            //尝试移除栈顶UI
            if ( !TryRemoveUI ( UI ) )
            {
                CLOG.E ( "the ui {0} is not the top ui in layout manager so can not destroy it!", UI.ToString() );
                return false;
            }

            if ( _UIStack.Count > 0 )
            {
                //新栈顶UI自动显示
                IUIBase UITop = _UIStack.Peek();
                if ( UITop != null )
                {
                    UITop.ShowUI();
                }
            }

            return true;
        }

        /// <summary>
        /// 尝试移除UI
        /// 返回是否从堆栈中成功移除
        /// 如果成功了
        /// 那么代表可以删除
        /// 如果移除失败了
        /// 那么代表不能删除
        /// </summary>
        /// <param name="UI"></param>
        public static bool TryRemoveUI ( IUIBase UI )
        {
            // 栈未初始化
            // 则默认可以删除UI
            // 因为此时UI尚未纳入层级管理
            // 因此可以自由删除
            if ( _UIStack == null )
            {
                return true;
            }

            // 栈不包含目标UI
            // 则默认可以删除UI
            // 因为此时UI尚未纳入层级管理
            // 因此可以自由删除
            if ( !_UIStack.Contains ( UI ) )
            {
                return true;
            }

            // 栈顶就是目标UI
            // 则可以删除UI
            // 因为栈只能从栈顶一个一个的删除
            if ( _UIStack.Peek() == UI )
            {
                _UIStack.Pop();
                return true;
            }

            // 其余情况无法删除UI
            // 原因是改UI处于UI层级管理器的管理
            // 且该UI不是栈顶UI
            // 因此不能删除
            CLOG.W ( "the ui {0} can not destroy because it is an layoutUI , its life was manager by UI Layout Manager", UI.ToString() );
            return false;
        }
    }
}