using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Text;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;


[System.Serializable]
public class UIClassMember
{
    public string MemberType;
    public string MemberName;
    public string MemberPath;
    public bool OutButtonEvent;
    public bool OutToggleEvent;
}

[System.Serializable]
public class UIClass
{
    public string ClassName;
    public string PrefabName;
    public string PrefabPath;
    public bool isSingleton;
    public bool isAnimation;
    public List<UIClassMember> members;
}


[System.Serializable]
public class EComponent
{
    public Component comp;
    public bool isUse;
    public bool canActive;
    public string compName;
    public Rect rect;

    public EComponent()
    {
        comp = null;
        compName = null;
        isUse = false;
        canActive = true;
    }
}

/// <summary>
/// UI节点
/// </summary>
[System.Serializable]
public class UINode
{
    public string name;
    public string path;
    public UINode parent;
    public bool isOn;
    public List<UINode> childs = null;
    public int layer;
    public int index;
    public List<EComponent> comps = null;
    public List<MonoBehaviour> scripts = null;
    public string prefabPath = null;
    public bool isAnimation = true;
    public bool isSingleton = true;

    public UINode()
    {
        name = "unname";
        isOn = false;
        childs = new List<UINode>();
        index = 0;
        layer = 0;
        parent = null;
        comps = new List<EComponent>();
        scripts = new List<MonoBehaviour>();
    }
}

/// <summary>
/// UI根节点
/// </summary>
[System.Serializable]
public class UIRoot : UINode { }


/// <summary>
/// UI清单
/// </summary>
[System.Serializable]
public class EData
{
    /// <summary>
    /// 所有UI
    /// </summary>
    public List<UIRoot> roots = null;

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
    /// 滚动区位置
    /// </summary>
    private Vector2 ScrollPos;

    /// <summary>
    /// 提示文字
    /// </summary>
    private GUIStyle hintLabelStyle;

    /// <summary>
    /// 鼠标是否按下
    /// </summary>
    private bool IsMouseDown = false;

    /// <summary>
    /// 存储的数据
    /// </summary>
    private EData _Data = new EData();
    private EData Data
    {
        get
        {
            if ( _Data == null )
            {
                _Data = new EData();
            }
            return _Data;
        }
    }

    /// <summary>
    /// 获得所有UI信息
    /// </summary>
    private void MakeAllUIInfo()
    {
        Data.roots = new List<UIRoot>();

        Transform Cans = GameObject.Find ( "Canvas" ).transform;

        if ( Cans == null )
        {
            EditorUtility.DisplayDialog ( "错误！", "本场景没有画布", "好的" );
            return;
        }

        for ( int i = 0; i < Cans.childCount; i++ )
        {
            Transform RT = Cans.GetChild ( i );

            if ( !RT.gameObject.activeInHierarchy )
            {
                continue;
            }

            UIRoot rt = new UIRoot();
            rt.name = RT.gameObject.name.Replace ( ' ', '_' );
            rt.isOn = true;
            rt.layer = 0;
            rt.index = i;
            rt.path = rt.name.Replace ( ' ', '_' );
            rt.parent = null;

            var prefab = PrefabUtility.GetPrefabObject ( RT.gameObject );
            if ( prefab == null )
            {
                EditorUtility.DisplayDialog ( "警告！", RT.gameObject.name + " 没有生成预制体", "知道了" );
                continue;
            }

            UnityEngine.Object parentObject = PrefabUtility.GetPrefabParent ( RT.gameObject );
            string path = AssetDatabase.GetAssetPath ( parentObject );
            rt.prefabPath = path.Substring ( 7 );

            Data.roots.Add ( rt );

            //添加孩子
            MakeUINodes ( rt, RT, rt.layer );
        }

        hintLabelStyle = EditorStyles.label;


        Debug.Log ( "场景UI加载成功" );
    }

    /// <summary>
    /// 生成并填充所有树数据
    /// </summary>
    /// <param name="parentNode"></param>
    /// <param name="parentTans"></param>
    private void MakeUINodes ( UINode parentNode, Transform parentTans, int parentLayerID )
    {
        for ( int i = 0; i < parentTans.childCount; i++ )
        {
            Transform RT = parentTans.GetChild ( i );

            UINode item = new UINode();
            item.name = RT.gameObject.name.Replace ( ' ', '_' );
            item.isOn = false;
            item.layer = parentLayerID + 1;
            item.index = i;
            item.path = parentNode.path + "/" + item.name;
            item.parent = parentNode;

            Component[] comps = RT.GetComponents<Component>();
            for ( int j = 0; j < comps.Length; j++ )
            {
                EComponent ecc = new EComponent();
                ecc.comp = comps[j];
                ecc.compName = comps[j].GetType().Name.Replace ( ' ', '_' );
                ecc.isUse = false;
                item.comps.Add ( ecc );
            }

            RT.GetComponents<MonoBehaviour> ( item.scripts );

            parentNode.childs.Add ( item );

            //添加孩子
            MakeUINodes ( item, RT, item.layer );
        }
    }


    /// <summary>
    /// TOOL工具
    /// </summary>
    [MenuItem ( "Tools/UI生成器",  priority = 200 )]
    private static void ShowTools()
    {
        // 弹出窗口
        s_Window = ( EUICreator ) EditorWindow.GetWindow ( typeof ( EUICreator ), false, "UI生成器", true );
        s_Window.Show();
    }

    /// <summary>
    /// 绘制GUI
    /// </summary>
    private void OnGUI()
    {
        if ( s_Window == null )
        {
            ShowTools();
        }

        if ( GUILayout.Button ( "加载场景UI", GUILayout.Height ( 30 ) ) )
        {
            MakeAllUIInfo();
        }

        if ( GUILayout.Button ( "导出类", GUILayout.Height ( 30 ) ) )
        {
            ExportAll();
        }

        if ( Event.current.type == EventType.MouseDown )
        {
            IsMouseDown = true;
        }

        if ( Event.current.type == EventType.MouseUp )
        {
            IsMouseDown = false;
        }

        if ( Data.roots == null ) { return; }

        ScrollPos = EditorGUILayout.BeginScrollView ( ScrollPos );
        for ( int i = 0; i < Data.roots.Count; i++ )
        {
            EditorGUILayout.BeginVertical ( EditorStyles.helpBox );

            EditorGUILayout.BeginVertical ( EditorStyles.miniButtonMid );
            Data.roots[i].isOn = EditorGUILayout.Foldout ( Data.roots[i].isOn, "----- [" + Data.roots[i].name + "] -----" );
            if ( Data.roots[i].isOn )
            {
                EditorGUILayout.BeginHorizontal ( GUILayout.Width ( 150f ) );
                if ( GUILayout.Button ( "全选" ) )
                {
                    SelectAll ( Data.roots[i], true );
                }

                if ( GUILayout.Button ( "全不选" ) )
                {
                    SelectAll ( Data.roots[i], false );
                }

                EditorGUILayout.EndHorizontal();

                Data.roots[i].isSingleton = EditorGUILayout.Toggle ( new GUIContent ( "是否单例UI" ), Data.roots[i].isSingleton );
                Data.roots[i].isAnimation = EditorGUILayout.Toggle ( new GUIContent ( "是否动画UI" ), Data.roots[i].isAnimation );
                EditorGUILayout.EndVertical();

                ShowChild ( Data.roots[i] );
            }
            else
            {
                EditorGUILayout.EndVertical();
            }

            EditorGUILayout.EndVertical();
        }

        EditorGUILayout.EndScrollView();
    }

    /// <summary>
    /// 显示孩子
    /// </summary>
    /// <param name="root"></param>
    private void ShowChild ( UINode root )
    {
        for ( int i = 0; i < root.childs.Count; i++ )
        {
            string blank = "";
            for ( int j = 0; j < root.childs[i].layer ; j++ )
            {
                blank += "   ";
            }

            string name = blank + root.childs[i].name;

            Rect rt = EditorGUILayout.BeginHorizontal();
            rt.height = 1;
            EditorGUI.DrawRect ( rt, new Color ( 0.3f, 0.3f, 0.3f, 0.5f ) );

            int width = ( int ) s_Window.position.width - root.childs[i].comps.Count * 16 - 34;
            root.childs[i].isOn = EditorGUILayout.ToggleLeft ( name, root.childs[i].isOn, GUILayout.Width ( width ) );

            DrawComponents ( root.childs[i] );
            EditorGUILayout.EndHorizontal();

            ShowChild ( root.childs[i] );
        }


    }

    public void OnInspectorUpdate()
    {
        this.Repaint();
    }

    /// <summary>
    /// 选择或者取消选择所有节点
    /// </summary>
    /// <param name="item"></param>
    /// <param name="isOn"></param>
    private void SelectAll ( UINode item, bool isOn )
    {
        for ( int i = 0; i < item.childs.Count; i++ )
        {
            item.childs[i].isOn = isOn;
            for ( int j = 0; j < item.childs[i].comps.Count; j++ )
            {
                item.childs[i].comps[j].isUse = isOn;
            }

            SelectAll ( item.childs[i], isOn );
        }
    }

    /// <summary>
    /// 绘制组件
    /// </summary>
    /// <param name="rect"></param>
    /// <param name="node"></param>
    private void DrawComponents ( UINode node )
    {
        Rect rect = EditorGUILayout.BeginHorizontal ();
        rect.width = 16;
        rect.height = 16;

        //绘制组件
        for ( int k = 0; k < node.comps.Count; k++ )
        {
            GUIContent content = EditorGUIUtility.ObjectContent ( node.comps[k].comp, null );

            Color color = GUI.color;
            color.a = node.comps[k].isUse ? 1f : 0.3f;
            GUI.color = color;
            GUI.DrawTexture ( rect, content.image );
            color.a = 1;
            GUI.color = color;

            if ( rect.Contains ( Event.current.mousePosition ) && Event.current.type == EventType.Repaint )
            {
                string componentName = node.comps[k].comp.GetType().Name.Replace ( ' ', '_' );

                Rect hintrect = rect;
                int labelWidth = Mathf.CeilToInt ( hintLabelStyle.CalcSize ( new GUIContent ( componentName ) ).x );
                hintrect.x = rect.x - labelWidth / 2 - 4;
                hintrect.width = labelWidth + 8;
                hintrect.height -= 1;

                if ( hintrect.y > 16 ) { hintrect.y -= 16; }
                else { hintrect.x += labelWidth; }

                EditorGUI.DrawRect ( hintrect, new Color ( 0f, 0f, 0f, 0.5f ) );
                hintrect.x += 4;
                hintrect.y += 1;
                GUI.Label ( hintrect, componentName, hintLabelStyle );
            }

            bool contain = rect.Contains ( Event.current.mousePosition );
            if ( contain && IsMouseDown && node.comps[k].canActive )
            {
                node.comps[k].isUse = !node.comps[k].isUse;
                node.comps[k].canActive = false;
            }

            if ( !contain && !node.comps[k].canActive  && !IsMouseDown )
            {
                node.comps[k].canActive = true;
            }

            rect.x += 16;
        }

        node.isOn = false;
        //绘制组件
        for ( int k = 0; k < node.comps.Count; k++ )
        {
            if ( node.comps[k].isUse )
            {
                node.isOn = true;
                break;
            }
        }

        EditorGUILayout.EndHorizontal();
    }

    /// <summary>
    /// 导出所有
    /// </summary>
    private void ExportAll()
    {
        if ( Data.roots == null )
        {
            EditorUtility.DisplayDialog ( "错误", "请先加载场景", "好的" );
            return;
        }

        string OutPutDir = "Assets/Scripts/UI/";
        string SceneName = EditorSceneManager.GetActiveScene().name;

        string FolderPath = OutPutDir + SceneName;

        DirectoryInfo di = new DirectoryInfo ( FolderPath );
        if ( !di.Exists )
        {
            di.Create();
        }

        for ( int i = 0; i < Data.roots.Count; i++ )
        {
            string filePath = FolderPath + "/V" + Data.roots[i].name + ".cs";
            WriteFile ( Data.roots[i], filePath );

            AssetDatabase.ImportAsset ( filePath, ImportAssetOptions.Default );
        }

    }

    /// <summary>
    /// 准备类对象
    /// </summary>
    /// <param name="ucls"></param>
    /// <param name="node"></param>
    private void MakeClassMember ( UIClass ucls, UINode node )
    {
        for ( int i = 0; i < node.comps.Count; i++ )
        {
            if ( node.comps[i].isUse )
            {
                UIClassMember uiCM = new UIClassMember();
                uiCM.MemberName = node.name.Replace ( ' ', '_' ) + "_" + node.comps[i].compName.Replace ( ' ', '_' );
                uiCM.MemberPath = node.path.Remove ( 0, ucls.PrefabName.Length + 1 );
                uiCM.MemberType = node.comps[i].compName.Replace ( ' ', '_' );
                uiCM.OutButtonEvent = node.comps[i].compName == "Button";
                uiCM.OutToggleEvent = node.comps[i].compName == "Toggle";

                ucls.members.Add ( uiCM );
            }
        }

        for ( int i = 0; i < node.childs.Count; i++ )
        {
            MakeClassMember ( ucls, node.childs[i] );
        }
    }

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="uIRoot"></param>
    private void WriteFile ( UIRoot root, string filePath )
    {
        UIClass ucls = new UIClass (  );
        ucls.ClassName = "V" + root.name.Replace ( ' ', '_' );
        ucls.PrefabName = root.name.Replace ( ' ', '_' );
        ucls.PrefabPath = root.prefabPath;
        ucls.members = new List<UIClassMember>();
        ucls.isAnimation = root.isAnimation;
        ucls.isSingleton = root.isSingleton;

        MakeClassMember ( ucls, root );

        WriteFile ( ucls, filePath );
        Debug.Log ( "Write CS File [" + filePath + "] Successful!" );

    }

    /// <summary>
    /// 写文件
    /// </summary>
    /// <param name="ucls"></param>
    private void WriteFile ( UIClass ucls, string filePath )
    {
        FileInfo fi = new FileInfo ( filePath );
        StreamWriter sw = fi.CreateText();
        sw.AutoFlush = true;
        sw.WriteLine ( "using UnityEngine;" );
        sw.WriteLine ( "using UnityEngine.UI;" );
        sw.WriteLine ( "using CoffeeBean;\r\n" );

        sw.WriteLine ( "/// <summary>" );
        sw.WriteLine ( "/// 请填写[ 类的作用 ]" );
        sw.WriteLine ( "/// </summary>" );
        sw.WriteLine ( "[CUIInfo(PrefabName = \"" + ucls.PrefabPath + "\", IsSigleton = " + ucls.isSingleton.ToString().ToLower() + ", IsAnimationUI = " + ucls.isAnimation.ToString().ToLower() + ", Description = \" 请填写[ 类的作用 ] \")]" );
        sw.WriteLine ( "public class " + ucls.ClassName  + " : CUIBase<" + ucls.ClassName + ">" );
        sw.WriteLine ( "{" );

        for ( int i = 0; i < ucls.members.Count; i++ )
        {
            sw.WriteLine ( "    /// <summary>" );
            sw.WriteLine ( "    /// 请填写[ 成员意义 ]" );
            sw.WriteLine ( "    /// </summary>" );
            sw.WriteLine ( "    private " + ucls.members[i].MemberType.Replace ( ' ', '_' ) + " " + ucls.members[i].MemberName.Replace ( ' ', '_' ) + ";\r\n" );
        }

        //Awake
        sw.WriteLine ( "    /// <summary>" );
        sw.WriteLine ( "    /// 苏醒" );
        sw.WriteLine ( "    /// </summary>" );
        sw.WriteLine ( "    private void Awake()" );
        sw.WriteLine ( "    {" );

        for ( int i = 0; i < ucls.members.Count; i++ )
        {
            sw.WriteLine ( "        " + ucls.members[i].MemberName.Replace ( ' ', '_' ) + " = transform.FindChildComponent< " + ucls.members[i].MemberType + " > ( \"" + ucls.members[i].MemberPath + "\" );" );
        }

        sw.WriteLine ( "        InitEvent();" );
        sw.WriteLine ( "    }\r\n" );


        //初始化事件
        sw.WriteLine ( "    /// <summary>" );
        sw.WriteLine ( "    /// 初始化事件" );
        sw.WriteLine ( "    /// </summary>" );
        sw.WriteLine ( "    private void InitEvent()" );
        sw.WriteLine ( "    {" );

        for ( int i = 0; i < ucls.members.Count; i++ )
        {
            if ( ucls.members[i].OutButtonEvent )
            {
                sw.WriteLine ( "        " + ucls.members[i].MemberName.Replace ( ' ', '_' ) + ".onClick.AddListener ( On_" + ucls.members[i].MemberName + "_Click );" );
            }
            else if ( ucls.members[i].OutToggleEvent )
            {
                sw.WriteLine ( "        " + ucls.members[i].MemberName.Replace ( ' ', '_' ) + ".onValueChanged.AddListener ( On_" + ucls.members[i].MemberName + "_Change );" );
            }
        }

        sw.WriteLine ( "    }\r\n" );

        //事件函数
        for ( int i = 0; i < ucls.members.Count; i++ )
        {
            string FunctionName = "";
            string param = "";
            if ( ucls.members[i].OutButtonEvent )
            {
                FunctionName = "On_" + ucls.members[i].MemberName.Replace ( ' ', '_' ) + "_Click";
            }
            else if ( ucls.members[i].OutToggleEvent )
            {
                FunctionName = "On_" + ucls.members[i].MemberName.Replace ( ' ', '_' ) + "_Change";
                param = "bool isOn";
            }
            else
            {
                continue;
            }

            sw.WriteLine ( "    /// <summary>" );
            sw.WriteLine ( "    /// " + ucls.members[i].MemberName + "点击事件" );
            sw.WriteLine ( "    /// </summary>" );
            sw.WriteLine ( "    private void " + FunctionName + "(" + param + ")" );
            sw.WriteLine ( "    {\r\n" );
            sw.WriteLine ( "    }\r\n" );
        }

        sw.WriteLine ( "}" );

        sw.Flush();
        sw.Close();
    }


}

