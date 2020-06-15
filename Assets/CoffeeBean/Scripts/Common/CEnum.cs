/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07 16:30
	File: 	    CEnum.cs
	Author:		Leo

	Purpose:	常用枚举

*********************************************************************/

namespace CoffeeBean
{
    /// <summary>
    /// 2方向
    /// </summary>
    public enum EDirection_2
    {
        /// <summary>
        /// 向左
        /// </summary>
        [CEnumDesc ( "向左" )]
        LEFT,

        /// <summary>
        /// 向右
        /// </summary>
        [CEnumDesc ( "向右" )]
        RIGHT,
    }

    /// <summary>
    /// 4方向
    /// </summary>
    public enum EDirection_4
    {
        /// <summary>
        /// 向上
        /// </summary>
        [CEnumDesc ( "向上" )]
        UP,

        /// <summary>
        /// 向下
        /// </summary>
        [CEnumDesc ( "向下" )]
        DOWN,

        /// <summary>
        /// 向左
        /// </summary>
        [CEnumDesc ( "向左" )]
        LEFT,

        /// <summary>
        /// 向右
        /// </summary>
        [CEnumDesc ( "向右" )]
        RIGHT,
    }

    /// <summary>
    /// 弹出框按钮样式
    /// </summary>
    public enum EPopupButtonType
    {
        /// <summary>
        /// 仅显示OK按钮
        /// </summary>
        OK_ONLY,

        /// <summary>
        /// OK和Cancel都显示
        /// </summary>
        OK_CANCEL,
    }

    /// 比率类型
    /// </summary>
    public enum EPrecentType
    {
        /// <summary>
        /// 百分比
        /// </summary>
        PRECENT_100 = 100,

        /// <summary>
        /// 千分比
        /// </summary>
        PRECENT_1000 = 1000,

        /// <summary>
        /// 万分比
        /// </summary>
        PRECENT_10000 = 10000
    }

    /// <summary>
    /// 显示状态枚举
    /// </summary>
    public enum EVisiableType
    {
        /// <summary>
        /// 隐藏
        /// </summary>
        [CEnumDesc ( "隐藏" )]
        HIDE,

        /// <summary>
        /// 显示
        /// </summary>
        [CEnumDesc ( "显示" )]
        SHOW
    }
}