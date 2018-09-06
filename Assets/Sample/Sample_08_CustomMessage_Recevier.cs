using UnityEngine;
using CoffeeBean;
using System;

public class Sample_08_CustomMessage_Recevier : MonoBehaviour, IMsgReceiver
{
    void Awake()
    {
        this.AddMessageHandler ( ECustomMessageType.SAMPLE_EVENT, OnTestEvent );
    }

    private void OnDestroy()
    {
        Debug.Log ( "移除注册的事件函数,若不移除，则下次派发时间时自动移除" );
        this.RemoveAllMessageHandler();
    }

    private void OnTestEvent ( object[] param )
    {
        for ( int i = 0; i < param.Length; i++ )
        {
            Debug.Log ( param[i] );
        }
    }
}
