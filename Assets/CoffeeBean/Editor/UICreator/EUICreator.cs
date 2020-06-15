using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;
using CoffeeBean;
using System.Reflection;
using DG.Tweening;

public enum EExport
{
    Export,
    NoExport
}

/// <summary>
/// UI编辑器窗体
/// </summary>
public class EUICreator : EditorWindow
{
    /// <summary>
    /// UI窗体
    /// </summary>
    private static EUICreator s_Window = null;

    /// <summary>
    /// 滚动位置
    /// </summary>
    private static Vector2 ScrollPos;

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

    /// <summary>
    /// TOOL工具
    /// </summary>
    [MenuItem( "Tools/UI生成器", priority = 200 )]
    private static void ShowTools()
    {
        s_Window = UnityEditor.EditorWindow.GetWindow<EUICreator>( false, "UI生成器", true );
        s_Window.minSize = new Vector2( 400, 800 );
        s_Window.Show();
    }

    // -------------------------------------------------------------------------------------
    // 面板方法
    // -------------------------------------------------------------------------------------

    private void Awake()
    {
        UIScaner.Clear();
    }

    /// <summary>
    /// 计算尺寸
    /// </summary>
    /// <param name="label"></param>
    /// <returns></returns>
    private int CalcSize( string label )
    {
        return Mathf.CeilToInt( EditorStyles.label.CalcSize( new GUIContent( label ) ).x );
    }

    /// <summary>
    /// 检查是否需要显示注释
    /// </summary>
    /// <param name="node"></param>
    /// <returns></returns>
    private bool checkNodeCanShowDesc( UINode node )
    {
        if ( node.export )
        {
            return true;
        }

        for ( int i = 0; i < node.comps.Count; i++ )
        {
            if ( node.comps[i].export )
            {
                return true;
            }
        }

        return false;
    }

    /// <summary>
    /// 绘制一条水平线
    /// </summary>
    private void DrawLine( int height = 2 )
    {
        GUILayout.Space( 5 );
        Rect rt = EditorGUILayout.BeginHorizontal();
        rt.height = 2;
        EditorGUI.DrawRect( rt, new Color( 0.3f, 0.3f, 0.3f, 0.5f ) );
        EditorGUILayout.EndHorizontal();
        GUILayout.Space( 5 );
    }

    /// <summary>
    /// 绘制节点
    /// </summary>
    /// <param name="node"></param>
    private void DrawNode( UINode node )
    {
        EditorGUILayout.BeginHorizontal();

        // 节点是否导出
        node.export = EditorGUILayout.Toggle( node.export, GUILayout.Width( 16 ) );

        // 节点是否导出
        var str = ' '.Repeat( node.level * 4 ) + node.node_name;
        EditorGUILayout.LabelField( str, GUILayout.Width( 150 ) );

        // 是否需要显示注释
        var showDesc = checkNodeCanShowDesc( node );

        // 显示组件
        ShowComp( node, node.level );

        if ( showDesc )
        {
            node.desc = EditorGUILayout.TextField( node.desc, GUILayout.Width( Mathf.CeilToInt( position.width * 0.2f ) ) );
        }

        EditorGUILayout.EndHorizontal();
        GUILayout.Space( 1 );
    }

    /// <summary>
    /// 绘制一个UI节点
    /// </summary>
    private void DrawUIRoot( UIRoot root )
    {
        var firstNode = root.nodes[0];

        // 标题区域
        EditorGUILayout.BeginHorizontal( EditorStyles.largeLabel, GUILayout.Height( 20 ) );
        EditorGUILayout.LabelField( "[" + firstNode.node_name + "]", GUILayout.Width( position.width - 48 ) );
        firstNode.export = EditorGUILayout.Toggle( firstNode.export ); // 确定节点是否导出
        EditorGUILayout.EndHorizontal();

        if ( !firstNode.export )
        {
            return;
        }

        EditorGUILayout.BeginHorizontal( EditorStyles.largeLabel, GUILayout.Height( 20 ) );

        // UI类型选择
        var index = GUILayout.Toolbar( root.info.uiType == EUIType.SINGLE? 1 : 0, new[] {"单体UI", "多重UI"} );
        root.info.uiType = index == 1 ? EUIType.SINGLE : EUIType.MUTI;

        // 导出方式选择
        index = GUILayout.Toolbar( root.info.exportType == EExportType.EXPORT_DEFINE ? 0 : 1, new[] { "仅导出定义", "导出所有" } );
        root.info.exportType = index == 0 ? EExportType.EXPORT_DEFINE : EExportType.EXPORT_ALL;
        EditorGUILayout.EndHorizontal();

        EditorGUILayout.BeginVertical( EditorStyles.helpBox );
        root.info.isModel = EditorGUILayout.Toggle( "是否模态UI", root.info.isModel );

        // 模态选择
        if ( root.info.isModel )
        {
            root.info.modelbackcolor = EditorGUILayout.ColorField( "模态背景色", root.info.modelbackcolor );
        }
        EditorGUILayout.EndVertical();

        // 显示所有子节点
        EditorGUILayout.BeginVertical( EditorStyles.textField );
        ShowRoot( root );
        EditorGUILayout.EndVertical();

        if ( GUILayout.Button( "快速导出", GUILayout.Height( 30 ) ) )
        {
        }
    }

    /// <summary>
    /// 绘制GUI
    /// </summary>
    private void OnGUI()
    {
        if ( GUILayout.Button( "加载场景UI", GUILayout.Height( 30 ) ) )
        {
            // 扫描场景UI
            UIScaner.ScanUI();
            return;
        }

        if ( !UIScaner.hasScanUI )
        {
            // 没有扫描过就算了
            return;
        }

        ScrollPos = EditorGUILayout.BeginScrollView( ScrollPos );

        // 绘制UI
        for ( int i = 0; i < UIScaner.roots.Count; i++ )
        {
            EditorGUILayout.BeginVertical( EditorStyles.helpBox );
            DrawUIRoot( UIScaner.roots[i] );
            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();

        EditorGUILayout.Space( 20 );
        if ( GUILayout.Button( "导出所有", GUILayout.Height( 30 ) ) )
        {
            // 创建所有UI
            UIWriter.MakeAllUI();
        }
    }

    /// <summary>
    /// 绘制组件
    /// </summary>
    private void ShowComp( UINode node, int level )
    {
        var rect = EditorGUILayout.BeginHorizontal();
        rect.width = 16;
        rect.height = 16;

        //绘制组件
        for ( int i = 0; i < node.comps.Count; i++ )
        {
            var comp = node.comps[i];

            var        type    = Assembly.Load(comp.assembly).GetType( comp.fullType );
            GUIContent content = EditorGUIUtility.ObjectContent( null, type );
            Color      color   = GUI.color;
            color.a = comp.export ? 1f : 0.3f;
            GUI.color = color;
            if ( content.image != null )
            {
                GUI.DrawTexture( rect, content.image );
            }
            else
            {
                GUI.DrawTexture( rect, EditorGUIUtility.IconContent( "cs Script Icon" ).image );
            }
            color.a = 1;
            GUI.color = color;
            var contain = rect.Contains( Event.current.mousePosition );

            // hint
            if ( contain && Event.current.type == EventType.Repaint )
            {
                Rect hintrect   = rect;
                int  labelWidth = Mathf.CeilToInt( EditorStyles.label.CalcSize( new GUIContent( comp.compType ) ).x );
                hintrect.x = rect.x - labelWidth / 2 - 4;
                hintrect.width = labelWidth + 8;
                hintrect.y -= 16;

                EditorGUI.DrawRect( hintrect, new Color( 0f, 0f, 0f, 0.5f ) );
                hintrect.x += 4;
                hintrect.y += 1;
                GUI.Label( hintrect, comp.compType, EditorStyles.label );
            }
            else if ( contain && Event.current.type == EventType.MouseDown )
            {
                comp.export = !comp.export;
            }

            rect.x += 16;
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 显示孩子
    /// </summary>
    /// <param name="root"></param>
    private void ShowRoot( UIRoot root )
    {
        for ( int i = 0; i < root.nodes.Count; i++ )
        {
            var node = root.nodes[i];
            DrawNode( node );
        }
    }
}