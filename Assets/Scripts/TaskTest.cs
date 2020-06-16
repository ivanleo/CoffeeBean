using System.Collections;
using System.Collections.Generic;
using CoffeeBean;
using UnityEngine;
using UnityEngine.TestTools;

public class TaskTest : MonoBehaviour
{
    public void Start()
    {
        // 简单任务的运行
        //new TaskTest1().Run();
        //new TaskTest2().Run();

        // 队列任务的运行
        var seq = new CTaskQueue();
        seq.Append( new TaskTest1() );
        seq.AppendCallFunc( () => Debug.Log( "1111111111111" ) );
        seq.AppendInteval( 3f );
        seq.Append( new TaskTest2() );
        seq.AppendCallFunc( () => Debug.Log( "2222222222222" ) );
        seq.Append( new TaskTest3() );
        seq.Run();
    }
}

/// <summary>
/// 测试任务
/// </summary>
public class TaskTest1 : CTask
{
    private int end = 5;
    private int i = 0;

    public override void OnFinish()
    {
        Debug.Log( $"Task:{ToString()} OnFinish:{i} frame:{Time.frameCount}" );
    }

    public override void OnStart()
    {
        Debug.Log( $"Task:{ToString()} OnStart:{i} frame:{Time.frameCount}" );
    }

    public override bool Update()
    {
        i++;
        Debug.Log( $"Task:{ToString()} Update:{i} frame:{Time.frameCount}" );
        return i >= end;
    }
}

/// <summary>
/// 测试任务
/// </summary>
public class TaskTest2 : CTask
{
    private int end = 105;
    private int i = 100;

    public override void OnFinish()
    {
        Debug.Log( $"Task:{ToString()} OnFinish:{i} frame:{Time.frameCount}" );
    }

    public override void OnStart()
    {
        Debug.Log( $"Task:{ToString()} OnStart:{i} frame:{Time.frameCount}" );
    }

    public override bool Update()
    {
        i++;
        Debug.Log( $"Task:{ToString()} Update:{i} frame:{Time.frameCount}" );
        return i >= end;
    }
}

/// <summary>
/// 测试任务3
/// </summary>
public class TaskTest3 : CTask
{
    public override async void OnStart()
    {
        Debug.Log( $"Task:{ToString()} Start frame:{Time.frameCount}" );
        await new WaitForSeconds( 5f );
        Finish();
    }
}