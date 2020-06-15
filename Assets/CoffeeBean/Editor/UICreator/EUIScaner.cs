using DG.Tweening;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

/// <summary>
/// UI扫描器
/// 负责扫描当前场景下所有UI并提炼数据
/// </summary>
public class UIScaner
{
    /// <summary>
    /// 是否已经扫描
    /// </summary>
    public static bool hasScanUI
    {
        get => roots != null && roots.Count > 0;
    }

    /// <summary>
    /// 扫描成功
    /// </summary>
    public static bool ScanSuccessful { get; private set; }

    /// <summary>
    /// 所有UI根节点
    /// </summary>
    public static List<UIRoot> roots { get; private set; }

    /// <summary>
    /// 清理
    /// </summary>
    public static void Clear()
    {
        ScanSuccessful = false;
        roots = new List<UIRoot>();
    }

    /// <summary>
    /// 扫描所有UI
    /// </summary>
    public static void ScanUI()
    {
        roots = new List<UIRoot>();
        Transform Cans = GameObject.Find( "Canvas" )?.transform;

        if ( Cans == null )
        {
            EditorUtility.DisplayDialog( "错误！", "本场景没有画布", "好的" );
            return;
        }

        for ( int i = 0; i < Cans.childCount; i++ )
        {
            Transform RT = Cans.GetChild( i );

            // 未激活的UI不进行扫描
            if ( !RT.gameObject.activeInHierarchy )
            {
                continue;
            }

            // 必须以UI_打头
            if ( !RT.name.StartsWith( "UI_" ) )
            {
                continue;
            }

            // 得到节点的预制体
            var prefab = PrefabUtility.GetPrefabInstanceHandle( RT.gameObject );
            if ( prefab == null )
            {
                EditorUtility.DisplayDialog( "警告！", RT.gameObject.name + " 没有生成预制体", "知道了" );
                continue;
            }

            var    uiName = RT.gameObject.name.Replace( ' ', '_' );
            UIRoot root   = new UIRoot();

            // 先看看文件夹在不在
            var di = new DirectoryInfo( "Assets/UIAssets" );
            if ( !di.Exists )
                di.Create();

            // 先看看有没有数据文件，有则读取数据文件，否则创建一个新的
            var     assetPath = "Assets/UIAssets/" + uiName + ".asset";
            EUIData assetData = null;
            if ( File.Exists( assetPath ) )
            {
                // 使用缓存的UI作为数据
                assetData = AssetDatabase.LoadAssetAtPath<EUIData>( assetPath );
            }

            UnityEngine.Object parentObject = PrefabUtility.GetCorrespondingObjectFromSource( RT.gameObject );

            root.info.prefab_path = AssetDatabase.GetAssetPath( parentObject );
            root.info.assetPath = assetPath;
            if ( assetData == null )
            {
                root.info.uiType = EUIType.SINGLE;
                root.info.exportType = EExportType.EXPORT_ALL;
                root.info.isModel = false;
                root.info.modelbackcolor = new Color( 0f, 0f, 0f, 0.6f );
            }
            else
            {
                var info = assetData.root.info;
                root.info.uiType = info.uiType;
                root.info.isModel = info.isModel;
                root.info.modelbackcolor = info.modelbackcolor;
                root.info.exportType = info.exportType;
            }

            // 扫描其下所有子节点
            ScanNodes( RT, 0, "", root.nodes );

            if ( assetData != null )
            {
                // 检查数据中是否有记载
                for ( int j = 0; j < root.nodes.Count; j++ )
                {
                    var node     = root.nodes[j];
                    // 复原节点选择状态
                    var findItem = assetData.root.nodes.Find( item => item.path == node.path && item.node_name == node.node_name );
                    if ( findItem != null )
                    {
                        node.export = findItem.export;
                        node.desc = findItem.desc;

                        // 复原组件选择状态
                        for ( int k = 0; k < node.comps.Count; k++ )
                        {
                            var comp     = node.comps[k];
                            var findComp = findItem.comps.Find( item => item.compType == comp.compType );
                            if ( findComp != null )
                            {
                                comp.export = findComp.export;
                            }
                        }
                    }
                }
            }

            roots.Add( root );
        }

        Debug.Log( "扫描场景UI成功！" );
    }

    /// <summary>
    /// 扫描组件
    /// </summary>
    private static void ScanComps( Transform target, List<UIComp> listComps )
    {
        Component[] comps = target.GetComponents<Component>();

        for ( int i = 0; i < comps.Length; i++ )
        {
            UIComp uicp = new UIComp();
            var    tp   = comps[i].GetType();
            uicp.compType = tp.Name;
            uicp.fullType = tp.FullName;
            uicp.assembly = tp.Assembly.FullName;

            // 自动导出按钮和toggle
            if ( uicp.compType == "Button" || uicp.compType == "Toggle" )
            {
                uicp.export = true;
            }

            listComps.Add( uicp );
        }
    }

    /// <summary>
    /// 获取子节点数据
    /// </summary>
    /// <param name="parentNode"></param>
    /// <param name="parentTans"></param>
    private static void ScanNodes( Transform target, int nodelevel, string parentPath, List<UINode> container )
    {
        var    nodeName = target.gameObject.name;
        UINode item     = new UINode();
        item.node_name = nodeName.Replace( ' ', '_' );

        if ( nodelevel == 0 )
        {
            item.path = "";
            item.export = true; // 默认导出
        }
        else if ( nodelevel == 1 )
            item.path = nodeName;
        else
            item.path = parentPath + "/" + nodeName;

        item.level = nodelevel;
        item.desc = "请输入注释";
        item.comps = new List<UIComp>();

        // 扫描组件
        ScanComps( target, item.comps );
        // 添加到容器
        container.Add( item );
        // 扫描子项
        for ( int i = 0; i < target.childCount; i++ )
        {
            var child = target.GetChild( i );
            // 扫描子节点
            ScanNodes( child, item.level + 1, item.path, container );
        }
    }
}