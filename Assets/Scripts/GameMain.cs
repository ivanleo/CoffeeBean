/********************************************************************
   All Right Reserved By Leo
   Created:    2020/6/16 9:15:08
   File: 	    GameMain.cs
   Author:     Leo

   Purpose:
*********************************************************************/

using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using UnityEngine;
using CoffeeBean;

public static class GameMain
{
    /// <summary>
    /// 是否已经初始化完毕
    /// </summary>
    public static bool HasInit { get; private set; }

    /// <summary>
    /// 游戏入口
    /// </summary>
    [RuntimeInitializeOnLoadMethod( RuntimeInitializeLoadType.BeforeSceneLoad )]
    public static void GameStart()
    {
        Debug.Log( "Game Start!" );

        // 初始化操作
        CLOG.Init();
        CApp.Inst.Init();

        // 设置第一个场景的绑定运行时类
        CSceneManager.ReadyToLoadSceneType = typeof( SampleScene );
        CSceneManager.Init();

        CApp.Inst.Looper += MainUpdate;
        CApp.Inst.SetDesignSize( 1280, 720 );
        CApp.Inst.TargetFrameRate = 60;
        CApp.Inst.MutiTouchEnabled = false;

        CFPS.Show();

        ResInit();
    }

    /// <summary>
    /// 游戏主循环
    /// </summary>
    public static void MainUpdate()
    {
#if UNITY_EDITOR
        if ( Input.GetKeyDown( KeyCode.F12 ) )
        {
            CFPS.Toggle();
        }
#endif
    }

    /// <summary>
    /// 资源初始化
    /// </summary>
    private async static void ResInit()
    {
        // 资源框架初始化
        await CRes.Init();

        // 检查要更新的列表
        //var CheckUpdateList = new List<object>();
        //CheckUpdateList.Add( "default" );
        //await CRes.CheckUpdate( CheckUpdateList );

        HasInit = true;
    }
}