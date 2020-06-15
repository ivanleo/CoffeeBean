using CoffeeBean;
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
/// UI书写器
/// </summary>
public class UIWriter
{
    /// <summary>
    /// 事件书写清单
    /// </summary>
    private static List<string> eventList;

    /// <summary>
    /// 函数清单
    /// </summary>
    private static List<string> funcList;

    /// <summary>
    /// 写类
    /// </summary>
    public static void MakeAllUI()
    {
        if ( UIScaner.roots == null )
        {
            EditorUtility.DisplayDialog( "错误", "请先加载场景", "好的" );
            return;
        }

        string outPutDir = "Assets/Scripts/UI/";
        string sceneName = EditorSceneManager.GetActiveScene().name;
        string folderPath = outPutDir + sceneName;

        DirectoryInfo di = new DirectoryInfo ( folderPath );
        if ( !di.Exists )
        {
            di.Create();
        }

        for ( int i = 0; i < UIScaner.roots.Count; i++ )
        {
            var root = UIScaner.roots[i];
            eventList = new List<string>();
            funcList = new List<string>();

            var firstNode = root.nodes[0];

            if ( !firstNode.export )
            {
                continue;
            }

            string definePath = folderPath + "/" + firstNode.node_name + "_Define.cs";
            string implementPath = folderPath + "/" + firstNode.node_name + ".cs";

            WriteDefine( root, definePath );

            if ( root.info.exportType == EExportType.EXPORT_ALL )
            {
                WriteImplement( root, implementPath );
            }

            AssetDatabase.ImportAsset( definePath, ImportAssetOptions.Default );
            AssetDatabase.ImportAsset( implementPath, ImportAssetOptions.Default );

            var data = ScriptableObject.CreateInstance<EUIData>();
            data.root = root;
            AssetDatabase.CreateAsset( data, root.info.assetPath );
            AssetDatabase.SaveAssets();
        }

        AssetDatabase.Refresh();
        EditorUtility.DisplayDialog( "成功", "生成成功", "好的" );
    }

    /// <summary>
    /// 创建UI定义类
    /// </summary>
    /// <param name="root"></param>
    /// <param name="definePath"></param>
    private static void WriteDefine( UIRoot root, string definePath )
    {
        var firstNode = root.nodes[0];

        FileInfo fi = new FileInfo ( definePath );
        StreamWriter sw = fi.CreateText();
        sw.AutoFlush = true;

        sw.WriteLine( "//" );
        sw.WriteLine( "// This is auto generate by UICreator" );
        sw.WriteLine( "// Please don't change it manual except you know what you do" );
        sw.WriteLine( "// Any question you can contact QQ:6828000" );
        sw.WriteLine( "//" );
        sw.WriteLine( "// Author: Leo" );
        sw.WriteLine( "//\r\n" );

        sw.WriteLine( "using UnityEngine;" );
        sw.WriteLine( "using UnityEngine.UI;" );
        sw.WriteLine( "using CoffeeBean;\r\n" );

        sw.WriteLine( "/// <summary>" );
        sw.WriteLine( "/// " + firstNode.desc );
        sw.WriteLine( "/// </summary>" );

        if ( root.info.uiType == EUIType.MUTI )
        {
            sw.WriteLine( "[CUIBind ( Prefab = \"" + root.info.prefab_path + "\" )]" );
        }
        else
        {
            var colorHex = ColorUtility.ToHtmlStringRGBA(root.info.modelbackcolor);
            sw.WriteLine( "[CUIBind ( Prefab = \"" + root.info.prefab_path + "\", IsModel = " + root.info.isModel.ToString().ToLower() + ", BackColor = \"#" + colorHex + "\" )]" );
        }

        // 父类
        var parentClass = root.info.uiType == EUIType.MUTI ? "CUIMuti" : "CUISingle";
        sw.WriteLine( "public partial class " + firstNode.node_name + " : " + parentClass + "<" + firstNode.node_name + ">" );
        sw.WriteLine( "{" );

        // 书写成员定义
        WriteMemberDefine( sw, root );

        sw.WriteLine( "    /// <summary>" );
        sw.WriteLine( "    /// 苏醒" );
        sw.WriteLine( "    /// </summary>" );
        sw.WriteLine( "    private void Awake()" );
        sw.WriteLine( "    {" );

        // 书写成员查找
        WriteMemberFind( sw, root );
        if ( eventList.Count > 0 )
        {
            sw.WriteLine( "\r\n        InitEvents();\r\n" );
        }
        sw.WriteLine( "    }\r\n" );

        if ( eventList.Count > 0 )
        {
            sw.WriteLine( "    /// <summary>" );
            sw.WriteLine( "    /// 事件注册" );
            sw.WriteLine( "    /// </summary>" );
            sw.WriteLine( "    private void InitEvents()" );
            sw.WriteLine( "    {" );
            for ( int i = 0; i < eventList.Count; i++ )
            {
                sw.WriteLine( eventList[i] );
            }
            sw.WriteLine( "    }\r\n" );
        }

        sw.WriteLine( "} // UI Define Class End" );
        sw.Flush();
        sw.Close();
    }

    /// <summary>
    /// 创建实现类
    /// </summary>
    private static void WriteImplement( UIRoot root, string implementPath )
    {
        var firstNode = root.nodes[0];

        FileInfo fi = new FileInfo ( implementPath );
        StreamWriter sw = fi.CreateText();
        sw.AutoFlush = true;

        sw.WriteLine( "//" );
        sw.WriteLine( "// This is auto generate by UICreator" );
        sw.WriteLine( "// You can code in this file" );
        sw.WriteLine( "// Any question you can contact QQ:6828000" );
        sw.WriteLine( "//" );
        sw.WriteLine( "// Author: Leo" );
        sw.WriteLine( "//\r\n" );

        sw.WriteLine( "using UnityEngine;" );
        sw.WriteLine( "using UnityEngine.UI;" );
        sw.WriteLine( "using CoffeeBean;\r\n" );

        sw.WriteLine( "/// <summary>" );
        sw.WriteLine( "/// " + firstNode.desc );
        sw.WriteLine( "/// </summary>" );

        // 父类
        var parentClass = root.info.uiType == EUIType.MUTI ? "CUIMuti" : "CUISingle";
        sw.WriteLine( "public partial class " + firstNode.node_name + " : " + parentClass + "<" + firstNode.node_name + ">" );
        sw.WriteLine( "{" );

        sw.WriteLine( "    /// <summary>" );
        sw.WriteLine( "    /// 开始时" );
        sw.WriteLine( "    /// </summary>" );
        sw.WriteLine( "    private void Start()" );
        sw.WriteLine( "    {" );
        sw.WriteLine( "        " );
        sw.WriteLine( "    }\r\n" );

        for ( int i = 0; i < funcList.Count; i++ )
        {
            var infos = funcList[i].Split ( '&' );
            var comment = infos[0];
            var func = infos[1];
            sw.WriteLine( "    /// <summary>" );
            sw.WriteLine( "    /// " + comment );
            sw.WriteLine( "    /// </summary>" );
            if ( func.EndsWith( "Click" ) )
            {
                sw.WriteLine( "    private void " + func + " ()" );
            }
            else if ( func.EndsWith( "Change" ) )
            {
                sw.WriteLine( "    private void " + func + " ( bool isOn )" );
            }
            sw.WriteLine( "    {" );
            sw.WriteLine( "        " );
            sw.WriteLine( "    }\r\n" );
        }

        sw.WriteLine( "} // UI Implement Class End" );
        sw.Flush();
        sw.Close();
    }

    /// <summary>
    /// 书写成员定义
    /// </summary>
    /// <param name="sw"></param>
    /// <param name="root"></param>
    private static void WriteMemberDefine( StreamWriter sw, UIRoot root )
    {
        // 遍历所有子节点
        for ( int i = 0; i < root.nodes.Count; i++ )
        {
            var node = root.nodes[i];
            if ( node.export && node.level > 0 )
            {
                var memberName = node.node_name.Replace ( " ", "_" );
                sw.WriteLine( "    /// <summary>" );
                sw.WriteLine( "    /// " + node.desc );
                sw.WriteLine( "    /// </summary>" );
                sw.WriteLine( "    private GameObject " + memberName + ";" );
            }

            // 根节点组件
            for ( int j = 0; j < node.comps.Count; j++ )
            {
                var comp = node.comps[j];
                if ( comp.export )
                {
                    var memberName = node.node_name.Replace ( " ", "_" ) + "_" + comp.compType;
                    sw.WriteLine( "    /// <summary>" );
                    sw.WriteLine( "    /// " + node.desc + " " + comp.compType );
                    sw.WriteLine( "    /// </summary>" );
                    sw.WriteLine( "    private " + comp.compType + " " + memberName + ";" );
                }
            }
        }
    }

    /// <summary>
    /// 书写子节点查找
    /// </summary>
    private static void WriteMemberFind( StreamWriter sw, UIRoot root )
    {
        // 遍历所有子节点
        for ( int i = 0; i < root.nodes.Count; i++ )
        {
            var node = root.nodes[i];
            if ( node.export && node.level > 0 )
            {
                var memberName = node.node_name.Replace ( " ", "_" );
                if ( node.level > 0 )
                {
                    sw.WriteLine( "        " + memberName + " = transform.Find ( \"" + node.path + "\" )?.gameObject;" );
                }
            }

            // 根节点组件
            for ( int j = 0; j < node.comps.Count; j++ )
            {
                var comp = node.comps[j];
                if ( comp.export )
                {
                    var memberName = node.node_name.Replace ( " ", "_" ) + "_" + comp.compType;
                    if ( node.level == 0 )
                    {
                        sw.WriteLine( "        " + memberName + " = transform.GetComponent <" + comp.compType + ">();" );
                    }
                    else
                    {
                        sw.WriteLine( "        " + memberName + " = transform.FindChildComponent<" + comp.compType + "> ( \"" + node.path + "\" );" );
                    }

                    // 缓存事件
                    if ( comp.compType == "Button" )
                    {
                        var clickFunc = "On_" + memberName + "_Click";
                        eventList.Add( "        " + memberName + ".onClick.AddListener(" + clickFunc + ");" );
                        funcList.Add( memberName + " 点击事件&" + clickFunc );
                    }
                    else if ( comp.compType == "Toggle" )
                    {
                        var changeFunc = "On_" + memberName + "_Change";
                        eventList.Add( "        " + memberName + ".onValueChanged.AddListener(" + changeFunc + ");" );
                        funcList.Add( memberName + " 改变事件&" + changeFunc );
                    }
                }
            }
        }
    }
}