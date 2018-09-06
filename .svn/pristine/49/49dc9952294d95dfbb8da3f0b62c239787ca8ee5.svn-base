/********************************************************************
    created:    2018/05/31
    created:    31:5:2018   11:33
    filename:   D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Attribute\CAttrReadOnly.cs
    file path:  D:\Work\PushCoin\trunk\PushCoin\Assets\CoffeeBean\Attribute
    file base:  CAttrReadOnly
    file ext:   cs
    author:     Leo

    purpose:    只读特性，编辑器下生效
*********************************************************************/
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 只读类
/// 控制某个属性在界面上不可以显示
/// </summary>
[AttributeUsage( AttributeTargets.Field )]
public class ReadOnly : PropertyAttribute
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer( typeof( ReadOnly ) )]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight( SerializedProperty property, GUIContent label )
    {
        return EditorGUI.GetPropertyHeight( property, label, true );
    }

    public override void OnGUI( Rect position, SerializedProperty property, GUIContent label )
    {
        GUI.enabled = false;
        EditorGUI.PropertyField( position, property, label, true );
        GUI.enabled = true;
    }
}
#endif