/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/08 10:46
	Filee: 	    CMsg.cs
	Author:		Leo

	Purpose:	自定义消息管理类
                使用方法

                class A {
                    void send(){
                        CMsg.Emit( "gamestart" , 111 , true , 33.33 , ()=>{ Debug.log(11); } );
                    }
                }

                class B {
                    B(){
                        // 只触发一次
                        // CMsg.Once( "gamestart" , OnGameStart );

                        // 每次都触发
                        CMsg.On( "gamestart" , OnGameStart );
                    }

                    void OnGameStart( params object[] param ){
                        // 遍历派发的事件参数
                        foreach(var item in param){
                            Debug.Log(item);
                        }
                    }
                }
*********************************************************************/

using System.Collections.Generic;

namespace CoffeeBean
{
    /// <summary>
    /// 自定义消息委托
    /// </summary>
    /// <param name="param">参数组</param>
    public delegate void DGCustomMessage( string msg, params object[] param );

    /// <summary>
    /// 消息处理器结构
    /// </summary>
    internal class LogicMsgHandler
    {
        /// <summary>
        /// 消息接受对象
        /// </summary>
        public object Receiver;

        /// <summary>
        /// 消息委托
        /// </summary>
        public DGCustomMessage Callback;

        /// <summary>
        /// 是否只触发一次
        /// </summary>
        public bool Once;

        public LogicMsgHandler( object receiver, DGCustomMessage Callback, bool once )
        {
            Receiver = receiver;
            this.Callback = Callback;
            Once = once;
        }
    }

    /// <summary>
    /// 消息管理类
    /// </summary>
    public static class CMsg
    {
        /// <summary>
        /// 消息注册管理对象
        /// </summary>
        private static Dictionary<string, List< LogicMsgHandler >> m_MsgHandlerList = new Dictionary<string, List<LogicMsgHandler>>();

        /// <summary>
        /// 禁用消息接受的物体清单
        /// </summary>
        private static List<object> m_DisableMsgObjs = new List<object>();

        /// <summary>
        /// 注册消息处理
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="msgStr"> 消息名 </param>
        /// <param name="callback"> 回调函数，无返回值，支持任意类型，任意长度的参数 </param>
        public static void On( object target, string msgStr, DGCustomMessage callback )
        {
            AddMessageHandler( target, msgStr, callback, false );
        }

        /// <summary>
        /// 注册只执行一次的消息处理
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="msgStr"></param>
        /// <param name="callback"></param>
        public static void Once( object target, string msgStr, DGCustomMessage callback )
        {
            AddMessageHandler( target, msgStr, callback, true );
        }

        /// <summary>
        /// 添加消息处理器
        /// </summary>
        private static void AddMessageHandler( object target, string msgStr, DGCustomMessage callback, bool once )
        {
            if ( !msgStr.IsNotNullAndEmpty() )
            {
                CLOG.E( "msg", $"error msg type is {msgStr}" );
                return;
            }

            if ( callback == null )
            {
                CLOG.E( "msg", "none call back!!" );
                return;
            }

            if ( !m_MsgHandlerList.ContainsKey( msgStr ) )
            {
                // 如果不包含这个Key，那么就创建他
                m_MsgHandlerList[msgStr] = new List<LogicMsgHandler>();
            }

            // 防止反复注册
            for ( int i = 0; i < m_MsgHandlerList[msgStr].Count; i++ )
            {
                LogicMsgHandler item = m_MsgHandlerList[msgStr][i];

                if ( item.Receiver.Equals( target ) && item.Callback == callback )
                {
                    return;
                }
            }

            // 注册
            m_MsgHandlerList[msgStr].Add( new LogicMsgHandler( target, callback, once ) );
        }

        /// <summary>
        /// 允许消息接受功能
        /// </summary>
        public static void EnableMsg( object target )
        {
            m_DisableMsgObjs.Remove( target );
        }

        /// <summary>
        /// 禁用消息接受
        /// </summary>
        /// <param name="self"></param>
        public static void DisableMsg( object target )
        {
            if ( !m_DisableMsgObjs.Contains( target ) )
            {
                m_DisableMsgObjs.Add( target );
            }
        }

        /// <summary>
        /// 移除掉某个对象身上的事件处理
        /// 一般在他的OnDestory里执行
        /// </summary>
        /// <param name="target">目标</param>
        /// <param name="msgStr">消息类型</param>
        public static void Off( object target, string msgStr )
        {
            if ( !msgStr.IsNotNullAndEmpty() )
            {
                CLOG.E( "msg", $"error msg type is {msgStr}" );
                return;
            }

            // 没有注册该消息时的处理
            if ( !m_MsgHandlerList.ContainsKey( msgStr ) )
            {
                return;
            }

            if ( target == null )
            {
                return;
            }

            // 得到所有注册的处理
            var Handlers = m_MsgHandlerList[msgStr];

            // 得到数量
            var HandlerCount = Handlers.Count;

            // 倒序遍历，防止删除引起的循环异常
            for ( int i = HandlerCount - 1; i >= 0; i-- )
            {
                var Handler = Handlers[i];

                // 存在处理对象才调用
                if ( Handler.Receiver.Equals( target ) )
                {
                    // 从禁用列表移除改对象
                    m_DisableMsgObjs.Remove( Handler.Receiver );
                    Handlers.Remove( Handler );
                }
            }
        }

        /// <summary>
        /// 移除一个对象身上建立的所有事件监听
        /// </summary>
        /// <param name="target">目标</param>
        public static void OffAll( object target )
        {
            // 遍历所有事件注册
            foreach ( var item in m_MsgHandlerList )
            {
                // 得到数量
                var HandlerCount = item.Value.Count;

                // 倒序遍历，防止删除引起的循环异常
                for ( int i = HandlerCount - 1; i >= 0; i-- )
                {
                    var Handler = item.Value[i];

                    // 存在处理对象才调用
                    if ( Handler.Receiver.Equals( target ) )
                    {
                        item.Value.Remove( Handler );
                    }
                }
            }

            // 从禁用列表移除改对象
            m_DisableMsgObjs.Remove( target );
        }

        /// <summary>
        /// 销毁时
        /// </summary>
        /// <param name="target"></param>
        public static void ClearMsg( object target )
        {
            OffAll( target );
        }

        /// <summary>
        /// 消息发送处理
        /// 注意
        /// 无论发送什么消息
        /// 其参数列表的0号下标都为消息名字符串
        /// 如
        ///     this.DispatchMessage( "test",123 );
        ///     this.AddMessageHandler( "test" ,OnTest );
        ///     private void OnTest( params object[] paramList ){
        ///         paramList[0]   // 固定为 "test";
        ///         paramList[1]   // 派发的第一个参数  123
        ///     }
        /// </summary>
        /// <param name="MsgType">要发送的消息类型</param>
        /// <param name="paramList">要发送的参数，支持任意类型，任意长度的参数</param>
        public static void Emit( string msgStr, params object[] paramList )
        {
            if ( !msgStr.IsNotNullAndEmpty() )
            {
                CLOG.E( "msg", $"error msg type is {msgStr}" );
                return;
            }

            // 没有注册该消息时的处理
            if ( !m_MsgHandlerList.ContainsKey( msgStr ) )
            {
                return;
            }

            // 得到所有注册的处理
            var Handlers = m_MsgHandlerList[msgStr];

            // 得到数量
            var HandlerCount = Handlers.Count;

            // 倒序遍历，防止删除引起的循环异常
            for ( int i = HandlerCount - 1; i >= 0; i-- )
            {
                var Handler = Handlers[i];

                // 存在处理对象才调用
                if ( !Handler.Receiver.Equals( null ) )
                {
                    // 被禁用消息接受的物体不能接受消息
                    if ( m_DisableMsgObjs.Contains( Handler.Receiver ) )
                    {
                        continue;
                    }

                    Handler.Callback.Invoke( msgStr, paramList );

                    if ( Handler.Once )
                    {
                        Handlers.Remove( Handler );
                    }
                }
                else
                {
                    // 目标变为空了
                    // 移除自己
                    Handlers.Remove( Handler );
                }
            }
        }
    }
}