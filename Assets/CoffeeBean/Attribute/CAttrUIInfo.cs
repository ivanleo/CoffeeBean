/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:32
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Attribute\CAttrUIPrefabBind.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Attribute
    file base:  CAttrUIPrefabBind
    file ext:   cs
    author:     Leo

    purpose:    UI预制体绑定特性
*********************************************************************/
using System;

namespace CoffeeBean
{
    /// <summary>
    /// UI特性
    /// 指定一个UI类的预制体
    /// 通过UI模组创建该UI时
    /// 会自动处理
    /// </summary>
    [AttributeUsage( AttributeTargets.Class, AllowMultiple = false )]
    public class CUIInfo : Attribute
    {
        /// <summary>
        /// UI预制体名字，UI预制体需要实现缓存起来
        /// </summary>
        private string m_PrefabName;

        /// <summary>
        /// 是否单例UI
        /// </summary>
        private bool m_IsSigleton;

        /// <summary>
        /// <summary>
        /// 是否是动画UI
        /// </summary>
        private bool m_IsAnimationUI;

        /// <summary>
        /// 描述
        /// </summary>
        private string m_Description;


        /// <summary>
        /// 无参构造，本特性的使用方法为
        /// </summary>
        public CUIInfo()
        {

        }

        /// <summary>
        /// 预制体名字
        /// 指定UI目录下的预制体名字，不要带后缀
        /// </summary>
        public string PrefabName
        {
            get { return m_PrefabName; }
            set { m_PrefabName = value; }
        }

        /// <summary>
        /// 是否单例UI
        /// 单例UI会保证在屏幕上只存在一个UI，如特定界面等
        /// 非单例UI则允许被创建多个来显示，如滚动区的节点，MessageBox等
        /// </summary>
        public bool IsSigleton
        {
            get { return m_IsSigleton; }
            set { m_IsSigleton = value; }
        }

        /// <summary>
        /// 描述
        /// </summary>
        public string Description
        {
            get { return m_Description; }
            set { m_Description = value; }
        }

        /// <summary>
        /// 是否是动画UI
        /// </summary>
        public bool IsAnimationUI
        {
            get { return m_IsAnimationUI; }
            set { m_IsAnimationUI = value; }
        }
    }
}
