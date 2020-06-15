using System;
using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// 游戏工具合集
/// </summary>
public class EGameTools : EditorWindow
{
    /// <summary>
    /// 游戏工具面板
    /// </summary>
    private static EGameTools myWindow = null;

    /// <summary>
    /// 显示游戏工具窗口
    /// </summary>
    [MenuItem( "Tools/游戏工具集", priority = 101 )]
    private static void ShowWindow()
    {
        // 弹出窗口
        myWindow = (EGameTools)EditorWindow.GetWindow( typeof( EGameTools ), false, "游戏工具集", true );
        myWindow.Show();
    }

    /// <summary>
    /// 激活时初始化
    /// </summary>
    private void OnEnable()
    {
        Init();
    }

    /// <summary>
    /// 分页表头
    /// </summary>
    private string [ ] m_ToolBarNames;

    /// <summary>
    /// 滚动位置
    /// </summary>
    private Vector2 m_ScrollPosition;

    /// <summary>
    /// 当前分页头
    /// </summary>
    private int m_NowSelectToolBar = 0;

    /// <summary>
    /// 工具清单
    /// </summary>
    private IGameTools [ ] Tools;

    /// <summary>
    /// 初始化
    /// </summary>
    private void Init()
    {
        m_ToolBarNames = new string[] { "场景切换器", "常用工具" };
        Tools = new IGameTools[m_ToolBarNames.Length];
        Tools[0] = new ESceneManager();
        Tools[1] = new EQuickTool();
        ParpareData();
    }

    /// <summary>
    /// 绘制窗口
    /// </summary>
    private void OnGUI()
    {
        // 绘制标题栏
        int temp = GUILayout.Toolbar ( m_NowSelectToolBar, m_ToolBarNames, GUILayout.Height ( 30 ) );

        if ( temp != m_NowSelectToolBar )
        {
            m_NowSelectToolBar = temp;
            ParpareData();
        }

        Tools[temp]?.Draw();
    }

    /// <summary>
    /// 准备数据
    /// </summary>
    private void ParpareData()
    {
        if ( Tools[m_NowSelectToolBar] == null )
        {
            EditorUtility.DisplayDialog( "错误", "当前选择分页没有处理对象", "好的" );
            return;
        }

        Tools[m_NowSelectToolBar].PerpareData();
    }

    /// <summary>
    /// 绘制一条线
    /// </summary>
    public static void DrawLine()
    {
        GUILayout.Space( 10 );
        Rect rt = EditorGUILayout.BeginHorizontal ( );
        rt.height = 4;
        EditorGUI.DrawRect( rt, new Color( 0.3f, 0.3f, 0.3f, 0.5f ) );
        EditorGUILayout.EndHorizontal();
        GUILayout.Space( 10 );
    }
}