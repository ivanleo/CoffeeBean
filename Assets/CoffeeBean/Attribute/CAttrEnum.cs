/********************************************************************
    created:    2017/10/26
    created:    26:10:2017   20:44
    filename:   D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Global\CAttributeCustom.cs
    file path:  D:\Work\NanJingMaJiang\trunk\Project\CoffeeBean\Global
    file base:  CAttributeCustom
    file ext:   cs
    author:     Leo

    purpose:    自定义特性 枚举描述
*********************************************************************/
using CoffeeBean;
using System;
using System.Linq;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 枚举描述特性
    /// </summary>
    [AttributeUsage ( AttributeTargets.Field )]
    public class CEnumDesc : Attribute
    {
        private string m_Desc = "";

        /// <summary>
        /// 无参构造，本特性的使用方法为
        /// [CEnumDescription( Description="XX枚举" )]
        /// </summary>
        public CEnumDesc()
        {

        }

        /// <summary>
        /// 有参构造，本特性的使用方法为
        /// [CEnumDescription( "XX枚举" )]
        /// </summary>
        public CEnumDesc ( string Desc )
        {
            m_Desc = Desc;
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Desc
        {
            get { return m_Desc; }
            set { m_Desc = value; }
        }
    }

    /// <summary>
    /// 显示枚举描述
    /// <para>public enum EXX{ AA,BB,CC };</para>
    /// <para>    public class C</para>
    /// <para>    {</para>
    /// <para>        [CShowEnum("XX状态")]</para>
    /// <para>        private EXX m_XXState;</para>
    /// <para>    }</para>
    /// </summary>
    public class CShowEnumDesc : HeaderAttribute
    {
        public CShowEnumDesc ( string EnumDesc ) : base ( EnumDesc ) { }
    }

    /// <summary>
    /// 枚举扩展ToString
    /// </summary>
    public static class EnumExpand
    {
        /// <summary>
        /// this扩展，给所有的枚举增加一个ToString方法用来返回特性描述中的值
        /// </summary>
        /// <param name="Target">枚举对象</param>
        public static string GetDescription ( this Enum Target )
        {
            try
            {
                Type EType = Target.GetType();
                string FieldName = Enum.GetName ( EType, Target );
                object[] Attributes = EType.GetField ( FieldName ).GetCustomAttributes ( false );
                CEnumDesc EnumDisplayAttribute = Attributes.FirstOrDefault ( ( p ) => { return p.GetType().Equals ( typeof ( CEnumDesc ) ); } ) as CEnumDesc;
                return EnumDisplayAttribute == null ? FieldName : EnumDisplayAttribute.Desc;
            }
            catch ( Exception ex )
            {
                CLOG.E ( ex.ToString() );
                return "";
            }

        }
    }
}
