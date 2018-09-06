﻿#pragma warning disable 1591

namespace CoffeeBean
{
    /// <summary>
    /// 自定义消息事件枚举，在这里注册消息类型
    /// 所有的自定义消息通过一个枚举来区分
    /// </summary>
    public enum ECustomMessageType
    {
        /// <summary>
        /// 禁止声音
        /// </summary>
        DISABLE_SOUND,

        /// <summary>
        /// 允许声音
        /// </summary>
        ABLE_SOUND,

        /// <summary>
        /// 示例事件
        /// </summary>
        SAMPLE_EVENT,

        /// <summary>
        /// 加速事件
        /// </summary>
        SPEED_UP,

        /// <summary>
        /// 恢复速度
        /// </summary>
        SPEED_NORMAL,

        NULL,
    }

    /// <summary>
    /// 显示状态枚举
    /// </summary>
    public enum EVisiableType
    {
        [CEnumDesc ( "隐藏状态" )]
        HIDE,
        [CEnumDesc ( "显示状态" )]
        SHOW
    }

    /// <summary>
    /// 2方向
    /// </summary>
    public enum EDirection_2
    {
        [CEnumDesc ( "向左" )]
        LEFT,
        [CEnumDesc ( "向右" )]
        RIGHT,

        NULL
    }

    /// <summary>
    /// 4方向
    /// </summary>
    public enum EDirection_4
    {
        [CEnumDesc ( "向上" )]
        UP,
        [CEnumDesc ( "向下" )]
        DOWN,
        [CEnumDesc ( "向左" )]
        LEFT,
        [CEnumDesc ( "向右" )]
        RIGHT,

        NULL
    }

    /// <summary>
    /// 弹出框按钮样式
    /// </summary>
    public enum EPopupButtonType
    {
        [CEnumDesc ( "仅有OK按钮" )]
        OK_ONLY,
        [CEnumDesc ( "OK按钮和Cencel按钮都有" )]
        OK_CANCEL,

        UNKNOWN
    }

    /// <summary>
    /// 比率类型
    /// </summary>
    public enum EPrecentType
    {
        /// <summary>
        /// 百分比
        /// </summary>
        [CEnumDesc ( "百分比" )]
        PRECENT_100 = 100,

        /// <summary>
        /// 千分比
        /// </summary>
        [CEnumDesc ( "千分比" )]
        PRECENT_1000 = 1000,

        /// <summary>
        /// 万分比
        /// </summary>
        [CEnumDesc ( "万分比" )]
        PRECENT_10000 = 10000
    }


    /**********************************************/
    /*         下面是本游戏的自定义枚举           */
    /**********************************************/
    #region CustomEnum


    #endregion
}