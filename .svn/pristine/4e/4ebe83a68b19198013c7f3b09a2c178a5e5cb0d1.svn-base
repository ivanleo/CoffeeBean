using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoffeeBean;
using System;

public class Sample_01_SingleFrameAnimation : MonoBehaviour
{
    private CSingleFrameAnimation CSFA = null;

    private void Awake()
    {
        CSFA = GetComponent<CSingleFrameAnimation>();
        CSFA.AddFrameCallBack ( 3, OnFrameCallBack );
        CSFA.SetEveryFrameCallBack ( OnEveryFrameCallBack );

        //若是CSingleFrameAnimation 没有设置AutoPlay可以用这句话播放
        //CSFA.PlayAnimation ( true );

    }

    private void OnEveryFrameCallBack ( int frameIndex )
    {
        Debug.Log ( "单动画组件 每一帧的回调被执行 当前帧:" + frameIndex );
    }

    private void OnFrameCallBack ( int frame )
    {
        Debug.Log ( "单动画组件 第 " + frame + "帧的回调被执行" );
    }

}
