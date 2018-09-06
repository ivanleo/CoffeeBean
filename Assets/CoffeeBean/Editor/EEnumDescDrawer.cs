/********************************************************************
	created:	2018/08/17
	created:	17:8:2018   10:55
	filename: 	D:\Work\CoffeeBean\Assets\CoffeeBean\Editor\EEnumDescDrawer.cs
	file path:	D:\Work\CoffeeBean\Assets\CoffeeBean\Editor
	file base:	EEnumDescDrawer
	file ext:	cs
	author:		Leo

	purpose:	枚举标签的显示方式
                public enum EPlayerState
                {
                    [CEnumDesc("待机")]
                    IDLE,
                    [CEnumDesc("死亡")]
                    DIE
                }

                [CShowEnumDesc("玩家状态")]
                public EPlayerState m_State = EPlayerState.DIE

                以上操作可以在 Inspector 中显示把枚举按照描述的文字显示出来
*********************************************************************/
using System.Collections.Generic;
using UnityEditor;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 枚举描述绘制器
    /// </summary>
    [CustomPropertyDrawer ( typeof ( CShowEnumDesc ) )]
    public class EEnumDescDrawer : PropertyDrawer
    {
        private readonly List<string> m_displayNames = new List<string>();

        public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
        {
            var att = ( CShowEnumDesc ) attribute;
            var type = property.serializedObject.targetObject.GetType();
            var field = type.GetField ( property.name );
            var enumtype = field.FieldType;

            foreach ( var enumName in property.enumNames )
            {
                //得到枚举的成员信息 FieldInfo
                var enumfield = enumtype.GetField ( enumName );

                string EnumDesc = null;
                object[] enumAttrs = enumfield.GetCustomAttributes ( false );
                foreach ( var item in enumAttrs )
                {
                    //遍历枚举类型来获得额外信息
                    if ( item.GetType() == typeof ( CEnumDesc ) )
                    {
                        EnumDesc = ( item as CEnumDesc ).Desc;
                        break;
                    }

                }
                if ( EnumDesc != null )
                {
                    m_displayNames.Add ( EnumDesc );
                }
            }

            EditorGUI.BeginChangeCheck();
            var value = EditorGUI.Popup ( position, att.header, property.enumValueIndex, m_displayNames.ToArray() );

            if ( EditorGUI.EndChangeCheck() )
            {
                property.enumValueIndex = value;
            }
        }
    }
}
