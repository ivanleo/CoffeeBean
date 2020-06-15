/********************************************************************
	All Right Reserved By Leo
	Created:	2019/01/07 16:31
	File: 	    CAttrReadOnly.cs
	Author:		Leo

	Purpose:	只读特性，编辑器下生效
                使用方法

                [ReadOnly]
                public int value = 10;

                则该值无法在 Inspector 中修改
*********************************************************************/
using UnityEngine;
using System;

#if UNITY_EDITOR
using UnityEditor;
#endif

/// <summary>
/// 只读类
/// 控制某个属性在界面上不可更改
/// </summary>
[AttributeUsage ( AttributeTargets.Field )]
public class CReadOnly : PropertyAttribute
{

}

#if UNITY_EDITOR
[CustomPropertyDrawer ( typeof ( CReadOnly ) )]
public class ReadOnlyDrawer : PropertyDrawer
{
    public override float GetPropertyHeight ( SerializedProperty property, GUIContent label )
    {
        return EditorGUI.GetPropertyHeight ( property, label, true );
    }

    public override void OnGUI ( Rect position, SerializedProperty property, GUIContent label )
    {
        GUI.enabled = false;
        EditorGUI.PropertyField ( position, property, label, true );
        GUI.enabled = true;
    }
}
#endif