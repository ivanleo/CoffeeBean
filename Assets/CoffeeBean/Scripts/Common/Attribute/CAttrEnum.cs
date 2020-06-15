/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07 16:30
	File: 	    CAttrEnum.cs
	Author:		Leo

	Purpose:	自定义特性 枚举描述
                使用方法

                public enum EPlayerState
                {
                    // 赋予枚举值特定字符
                    [CEnumDesc("待机")]
                    IDLE,
                    [CEnumDesc("死亡")]
                    DIE
                }

                // 令本枚举值在 Inspector 中不再显示枚举名，而是显示赋予的特定字符
                // 以本例 显示为    玩家状态    待机
                [CShowEnumDesc("玩家状态")]
                public EPlayerState m_State = EPlayerState.DIE

                // 获得枚举的特定字符
                // desc = "待机"
                var desc = m_State.GetDescription();

*********************************************************************/

using System;
using System.Linq;

using UnityEngine;
using System.Collections.Generic;

#if UNITY_EDITOR

using UnityEditor;

#endif

namespace CoffeeBean
{
    /// <summary>
    /// 枚举描述特性
    /// </summary>
    [AttributeUsage( AttributeTargets.Field )]
    public class CEnumDesc : Attribute
    {
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
        public CEnumDesc( string Desc )
        {
            this.Desc = Desc;
        }

        /// <summary>
        /// 枚举描述
        /// </summary>
        public string Desc { get; set; } = "";
    }

    /// <summary>
    /// 枚举扩展一个GetDescription 方法
    /// </summary>
    public static class EnumExpand
    {
        /// <summary>
        /// this扩展，给所有的枚举增加一个 GetDescription 方法用来返回特性描述中的值
        /// </summary>
        /// <param name="Target">枚举对象</param>
        public static string GetDescription( this Enum Target )
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
                CLOG.E( ex.ToString() );
                return "";
            }
        }
    }

    /// <summary>
    /// 显示枚举描述
    /// public enum EXX{ AA,BB,CC };
    ///     public class
    ///     {
    ///         [CShowEnum("XX状态")]
    ///         private EXX m_XXState;
    ///     }
    /// </summary>
    public class CShowEnumDesc : HeaderAttribute
    {
        public CShowEnumDesc( string EnumDesc ) : base( EnumDesc )
        {
        }
    }

#if UNITY_EDITOR

    /// <summary>
    /// <para>枚举绘制器</para>
    /// <para>允许在 Inspector总显示描述支符</para>
    /// <para>枚举标签的显示方式</para>
    /// <example>
    /// <code>
    /// <para>public enum EPlayerState</para>
    /// <para>{</para>
    /// <para>[CEnumDesc("待机")]</para>
    /// <para>IDLE,</para>
    /// <para>[CEnumDesc("死亡")]</para>
    /// <para>DIE</para>
    /// <para>}</para>
    /// <para></para>
    /// <para>[CShowEnumDesc("玩家状态")]</para>
    /// <para>public EPlayerState m_State = EPlayerState.DIE</para>
    /// </code>
    /// 以上操作可以在 Inspector 中显示把枚举按照描述的文字显示出来
    /// </example>
    /// </summary>

    [CustomPropertyDrawer( typeof( CShowEnumDesc ) )]
    public class EEnumDescDrawer : PropertyDrawer
    {
        /// <summary>
        /// 要显示的字符清单
        /// </summary>
        /// <typeparam name="string"></typeparam>
        /// <returns></returns>
        private readonly List<string> m_displayNames = new List<string>( );

        /// <summary>
        /// 绘制方法
        /// </summary>
        /// <param name="position"></param>
        /// <param name="property"></param>
        /// <param name="label"></param>
        public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
        {
            var att = ( CShowEnumDesc ) attribute;
            var type = property.serializedObject.targetObject.GetType( );
            var field = type.GetField ( property.name );
            var enumtype = field.FieldType;

            // 便利枚举
            for ( int i = 0; i < property.enumNames.Length; i++ )
            {
                string enumName = property.enumNames[i];
                // 得到枚举的成员信息 FieldInfo
                var enumfield = enumtype.GetField ( enumName );
                string EnumDesc = null;
                // 得到特性
                object[ ] enumAttrs = enumfield.GetCustomAttributes ( false );

                // 便利特性
                for ( int j = 0; j < enumAttrs.Length; j++ )
                {
                    object item = enumAttrs[j];
                    //遍历枚举类型来获得额外信息
                    if ( item.GetType() == typeof( CEnumDesc ) )
                    {
                        EnumDesc = ( item as CEnumDesc ).Desc;
                        break;
                    }
                }

                if ( EnumDesc != null )
                {
                    m_displayNames.Add( EnumDesc );
                }
            }

            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Popup ( position, att.header, property.enumValueIndex, m_displayNames.ToArray( ) );

            if ( EditorGUI.EndChangeCheck() )
            {
                property.enumValueIndex = value;
            }
        }
    }

#endif
}