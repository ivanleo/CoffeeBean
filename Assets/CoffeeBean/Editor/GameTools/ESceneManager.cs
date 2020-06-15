using System.Collections.Generic;
using System.IO;
using UnityEditor;
using UnityEditor.SceneManagement;
using UnityEngine;

public class ESceneManager: IGameTools
{
    /// <summary>
    /// 场景文件夹
    /// </summary>
    private const string SCENE_FOLDER = "Assets/Scenes";

    // 场景数据
    private struct SceneData
    {
        public string Name;
        public string SceneURL;
    }

    /// <summary>
    /// 场景资源清单
    /// Key 场景名 value 场景资源路径
    /// </summary>
    private Dictionary<string, List<SceneData>> m_GameScenes = null;

    /// <summary>
    /// 场景标头
    /// </summary>
    private string[] SceneTitles;

    /// <summary>
    /// 默认表头
    /// </summary>
    private const string DEFAULT_TITLE = "Root";

    /// <summary>
    /// 滚动位置
    /// </summary>
    private Vector2 m_ScrollPosition;

    /// <summary>
    /// 当前分页头
    /// </summary>
    private int m_NowSelectToolBar = 0;

    /// <summary>
    /// 准备数据
    /// </summary>
    public void PerpareData()
    {
        // 场景文件信息
        m_GameScenes = new Dictionary<string, List<SceneData>>();

        //获得SceneURL路径下目录的名字
        DirectoryInfo di = new DirectoryInfo ( SCENE_FOLDER );
        DirectoryInfo[] dis = di.GetDirectories();

        // 生成表头
        m_GameScenes.Add ( DEFAULT_TITLE, new List<SceneData>() );
        int i = 0;

        for ( i = 0; i < dis.Length; i++ )
        {
            m_GameScenes.Add ( dis[i].Name, new List<SceneData>() );
        }

        // 获取所有场景
        string[] Guids = AssetDatabase.FindAssets ( "t:Scene", new string[] { SCENE_FOLDER } );

        // 转路径
        for ( i = 0; i < Guids.Length; i++ )
        {
            Guids[i] = AssetDatabase.GUIDToAssetPath ( Guids[i] );
            string[] temps = Guids[i].Split ( '/' );

            string folderName = temps[2];
            UnityEngine.Object data = AssetDatabase.LoadAssetAtPath<UnityEngine.Object> ( Guids[i] );

            SceneData ssd = new SceneData();
            ssd.Name = data.name;
            ssd.SceneURL = Guids[i];

            if ( m_GameScenes.ContainsKey ( folderName ) )
            {

                m_GameScenes[folderName].Add ( ssd );
            }
            else
            {
                m_GameScenes[DEFAULT_TITLE].Add ( ssd );
            }

            Resources.UnloadAsset ( data );
        }

        // 生成类型名
        var titleList = new List<string>();
        i = 0;

        foreach ( KeyValuePair<string, List<SceneData>> item in m_GameScenes )
        {
            if ( item.Value.Count > 0 )
            {
                titleList.Add ( item.Key );
            }
        }

        SceneTitles = titleList.ToArray();
    }

    public void Draw()
    {
        // 绘制标题栏
        int temp = GUILayout.Toolbar ( m_NowSelectToolBar, SceneTitles, GUILayout.Height ( 30 ), GUILayout.Width ( SceneTitles .Length * 60f ) );

        if ( temp != m_NowSelectToolBar )
        {
            PerpareData();
            m_NowSelectToolBar = temp;
        }


        m_ScrollPosition = EditorGUILayout.BeginScrollView ( m_ScrollPosition );

        List<SceneData> SceneList = m_GameScenes[SceneTitles[m_NowSelectToolBar]];

        for ( int i = 0; i < SceneList.Count; i ++ )
        {
            if ( GUILayout.Button ( SceneList[i].Name, GUILayout.Height ( 25 ) ) )
            {
                EditorSceneManager.OpenScene ( SceneList[i].SceneURL );
            }
        }

        //结束一个滚动区域
        EditorGUILayout.EndScrollView();
    }
}


