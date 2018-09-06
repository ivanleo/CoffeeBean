/********************************************************************
	created:	2018/08/17
	created:	17:8:2018   10:59
	filename: 	D:\Work\CoffeeBean\Assets\CoffeeBean\Editor\EFrameDrawer.cs
	file path:	D:\Work\CoffeeBean\Assets\CoffeeBean\Editor
	file base:	EFrameDrawer
	file ext:	cs
	author:		Leo

	purpose:	动画帧的绘制器
                负责序列帧动画的绘制功能
*********************************************************************/
using UnityEditor;
using UnityEngine;

namespace CoffeeBean
{
    /// <summary>
    /// 自定义帧Inspector显示
    /// </summary>
    [CustomPropertyDrawer ( typeof ( CFrame ) )]
    public class EFrameDrawer : PropertyDrawer
    {
        public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
        {
            SerializedProperty SpFrame = property.FindPropertyRelative ( "SpFrame" );
            SerializedProperty SpInteval = property.FindPropertyRelative ( "SpInteval" );

            string[] labels = label.text.Split ( ' ' );
            label.text = string.Format ( "Frame {0}", labels[1] );

            //编辑器宽
            float LabelWidth = EditorGUIUtility.labelWidth - 12;
            var labelRect = new Rect ( position.x, position.y, LabelWidth, position.height );
            var SpIntevalRect = new Rect ( position.width - 32, position.y, 45, position.height );
            var SpFrameRect = new Rect ( position.x + LabelWidth, position.y, position.width - 40 - LabelWidth, position.height );

            EditorGUIUtility.labelWidth = 12.0f;
            EditorGUI.LabelField ( labelRect, label );
            EditorGUI.PropertyField ( SpFrameRect, SpFrame );
            EditorGUI.PropertyField ( SpIntevalRect, SpInteval );
            EditorGUIUtility.labelWidth = LabelWidth;
        }
    }

}
