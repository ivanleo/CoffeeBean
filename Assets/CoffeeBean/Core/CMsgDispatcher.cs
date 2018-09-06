/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:41
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\CustomMessage\CCustomMessageManager.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\CustomMessage
    file base:  CCustomMessageManager
    file ext:   cs
    author:     Leo

    purpose:    自定义消息管理类
*********************************************************************/
using System.Collections.Generic;
using System;

namespace CoffeeBean
{
    /// <summary>
    /// 自定义消息委托
    /// </summary>
    /// <param name="param">参数组</param>
    public delegate void DelegateCustomMessage ( params object[] param );

    /// <summary>
    /// 消息接受接口，但凡继承本接口则具备接受消息的能力
    /// </summary>
    public interface IMsgReceiver {}

    /// <summary>
    /// 消息发送者接口，但凡继承本接口则具备发送消息的能力
    /// </summary>
    public interface IMsgSender {}

    /// <summary>
    /// 消息派发器类
    /// </summary>
    public static class CMsgDispatcher
    {
        /// <summary>
        /// 消息捕捉器
        /// 记录消息名和处理回调
        /// </summary>
        private class LogicMsgHandler
        {
            // 消息接受对象
            public IMsgReceiver m_Receiver;
            public DelegateCustomMessage m_Callback;

            public LogicMsgHandler ( IMsgReceiver receiver, DelegateCustomMessage Callback )
            {
                m_Receiver = receiver;
                m_Callback = Callback;
            }
        }

        // 消息注册管理对象
        private static Dictionary<ECustomMessageType, List< LogicMsgHandler >> m_MsgHandlerList = new Dictionary<ECustomMessageType, List<LogicMsgHandler>>();

        /// <summary>
        /// 注册消息处理
        /// </summary>
        /// <param name="self">this扩展，this必须是 ImsgReceiver 的实例</param>
        /// <param name="MsgType"> 消息名 </param>
        /// <param name="Callback"> 回调函数，无返回值，支持任意类型，任意长度的参数 </param>
        public static void AddMessageHandler ( this IMsgReceiver self, ECustomMessageType MsgType, DelegateCustomMessage Callback )
        {
            if ( MsgType == ECustomMessageType.NULL )
            {
                CLOG.E ( "error msg type is {0}", MsgType );
                return;
            }

            if ( Callback == null )
            {
                CLOG.E ( "none call back!!", MsgType );
                return;
            }

            if ( !m_MsgHandlerList.ContainsKey ( MsgType ) )
            {
                // 如果不包含这个Key，那么就创建他
                m_MsgHandlerList[MsgType] = new List<LogicMsgHandler>();
            }

            // 防止反复注册
            foreach ( var item in m_MsgHandlerList[MsgType] )
            {
                if ( item.m_Receiver == self && item.m_Callback == Callback )
                {
                    return;
                }
            }

            // 注册
            m_MsgHandlerList[MsgType].Add ( new LogicMsgHandler ( self, Callback ) );
        }

        /// <summary>
        /// 移除掉某个对象身上的事件处理
        /// 一般在他的OnDestory里执行
        /// </summary>
        /// <param name="self">this 扩展，扩展对象必须是IMsgReceiver 的实例</param>
        /// <param name="MsgType">消息类型</param>
        public static void RemoveMessageHandler ( this IMsgReceiver self, ECustomMessageType MsgType )
        {
            if ( MsgType == ECustomMessageType.NULL )
            {
                CLOG.E ( "error msg type is {0}", MsgType );
                return;
            }

            // 没有注册该消息时的处理
            if ( !m_MsgHandlerList.ContainsKey ( MsgType ) )
            {
                return;
            }

            // 得到所有注册的处理
            var Handlers = m_MsgHandlerList[MsgType];

            // 得到数量
            var HandlerCount = Handlers.Count;

            // 倒序遍历，防止删除引起的循环异常
            for ( int i = HandlerCount - 1; i >= 0; i-- )
            {
                var Handler = Handlers[i];

                // 存在处理对象才调用
                if ( Handler.m_Receiver == self )
                {
                    Handlers.Remove ( Handler );
                }
            }
        }

        /// <summary>
        /// 移除一个对象身上建立的所有事件监听
        /// </summary>
        /// <param name="self">this 扩展，必须是IMsgReceiver的实例 </param>
        public static void RemoveAllMessageHandler ( this IMsgReceiver self )
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
                    if ( Handler.m_Receiver == self )
                    {
                        item.Value.Remove ( Handler );
                    }
                }

            }

        }

        /// <summary>
        /// 消息发送处理
        /// </summary>
        /// <param name="self">this扩展，this必须是 IMsgSender的实例</param>
        /// <param name="MsgType">要发送的消息类型</param>
        /// <param name="paramList">要发送的参数，支持任意类型，任意长度的参数</param>
        public static void DispatchMessage ( this IMsgSender self, ECustomMessageType MsgType, params Object[] paramList )
        {
            if ( MsgType == ECustomMessageType.NULL )
            {
                CLOG.E ( "error msg type is {0}", MsgType );
                return;
            }

            // 没有注册该消息时的处理
            if ( !m_MsgHandlerList.ContainsKey ( MsgType ) )
            {
                return;
            }

            // 得到所有注册的处理
            var Handlers = m_MsgHandlerList[MsgType];

            // 得到数量
            var HandlerCount = Handlers.Count;

            // 倒序遍历，防止删除引起的循环异常
            for ( int i = HandlerCount - 1; i >= 0; i-- )
            {
                var Handler = Handlers[i];

                // 存在处理对象才调用
                if ( Handler.m_Receiver != null )
                {
                    Handler.m_Callback ( paramList );
                }
                else
                {
                    Handlers.Remove ( Handler );
                }
            }
        }
    }
}