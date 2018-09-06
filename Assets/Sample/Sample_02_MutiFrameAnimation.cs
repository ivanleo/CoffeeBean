using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoffeeBean;
using System;

public class Sample_02_MutiFrameAnimation : MonoBehaviour
{
    private CMutiFrameAnimation CMFA = null;

    private void Awake()
    {
        CMFA = GetComponent<CMutiFrameAnimation>();

        CMFA.AddFrameCallBack ( "Swim", 3, OnFrameCallBack );
        CMFA.AddFrameCallBack ( 0, 1, OnFrameCallBack );

        CMFA.SetEveryFrameCallBack ( "Swim", OnEveryFrameCallBack );
        CMFA.SetEveryFrameCallBack ( 1, OnEveryFrameCallBack );

    }

    private void OnEveryFrameCallBack ( int frameIndex )
    {
        Debug.Log ( "多状态动画组件 每一帧的回调被执行 当前帧:" + frameIndex );
    }

    private void OnFrameCallBack ( int frame )
    {
        Debug.Log ( "多状态动画组件 第 " + frame + "帧的回调被执行" );
    }

    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 200, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "播放游泳动画", gs ) )
        {
            CMFA.PlayAnimation ( 0, true );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "播放挣扎动画", gs ) )
        {
            CMFA.PlayAnimation ( "Struggle", true );
        }

    }

}
