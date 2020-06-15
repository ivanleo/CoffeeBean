/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/12 14:24:37
   File: 	    Scene.cs
   Author:     Leo

   Purpose:
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using CoffeeBean;
using UnityEngine;
using UnityEngine.SceneManagement;

public class Scene1 : CSceneBase
{
    private int i = 0;

    public override void AfterEnterScene( Scene scene )
    {
        Debug.Log( "Scene1 AfterEnterScene" );
    }

    public override void BeforeLeftScene( Scene scene )
    {
        Debug.Log( "Scene1 BeforeLeftScene" );
    }

    public override void SceneUpdate()
    {
        //Debug.Log( "Scene1 SceneUpdate" );
        i++;
        if ( i == 240 )
        {
            CSceneManager.LoadScene<Scene2>( "Test2", ( float progress ) => Debug.Log( $"Loading Test2:{progress}" ) );
        }
    }
}

public class Scene2 : CSceneBase
{
    public override void AfterEnterScene( Scene scene )
    {
        Debug.Log( "Scene2 AfterEnterScene" );
    }

    public override void BeforeLeftScene( Scene scene )
    {
        Debug.Log( "Scene2 BeforeLeftScene" );
    }

    public override void SceneUpdate()
    {
        Debug.Log( "Scene2 SceneUpdate" );
    }
}

public class SceneTest : MonoBehaviour
{
    public void Start()
    {
        CoffeeMain.Init();
    }

    private void LoadScene()
    {
        CSceneManager.LoadScene<Scene1>( "Test1", ( float progress ) => Debug.Log( $"Loading Test1:{progress}" ) );
    }

    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 100, 100, 200, 60 ), "加载新地图" ) )
        {
            LoadScene();
        }
    }
}