using CoffeeBean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Mission_1 : CMission
{
    public override void OnStart()
    {
        CCoroutineManager.Instance.DoCoroutine ( Count() );
    }

    private IEnumerator Count()
    {
        int i = 0;
        while ( i < 10 )
        {
            Debug.Log ( i );
            yield return new WaitForSeconds ( 0.5f );
            i++;
        }

        Finish();
    }
}

public class Mission_2 : CMission
{
    public override void OnStart()
    {
        CCoroutineManager.Instance.DoCoroutine ( Count() );
    }

    private IEnumerator Count()
    {
        int i = 100;
        while ( i < 110 )
        {
            Debug.Log ( i );
            yield return new WaitForSeconds ( 0.5f );
            i++;
        }

        Finish();
    }
}

public class Mission_3 : CMission
{
    public override void OnStart()
    {
        CCoroutineManager.Instance.DoCoroutine ( Count() );
    }

    private IEnumerator Count()
    {
        int i = 200;
        while ( i < 210 )
        {
            Debug.Log ( i );
            yield return new WaitForSeconds ( 0.5f );
            i++;
        }

        Finish();
    }
}

public class Mission_4 : CMission
{
    public override void OnStart()
    {
        CCoroutineManager.Instance.DoCoroutine ( Count() );
    }

    private IEnumerator Count()
    {
        int i = 300;
        while ( i < 310 )
        {
            Debug.Log ( i );
            yield return new WaitForSeconds ( 0.5f );
            i++;
        }

        Finish();
    }
}


public class Sample_04_MissionSystem : MonoBehaviour
{
    private void Start()
    {
        CMissionSystem.Instance.NeedLog = true;
    }

    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 300, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "开始一个任务", gs ) )
        {
            //单任务执行
            new Mission_1().Start();
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "开始一个任务 + 完成回调", gs ) )
        {
            //单任务执行带完成回调
            new Mission_2().Start ( () => { Debug.Log ( "Mission_2 Complete" ); } );
        }

        rt.y += 80f;
        if ( GUI.Button ( rt, "开始一个任队列", gs ) )
        {
            //任务队列
            CMissionSequence Seq = CMissionSystem.CreateSequence();
            Seq.Append ( new Mission_1() );
            Seq.Append ( new Mission_2() );
            Seq.Join ( new Mission_3() );
            Seq.AppendInteval ( 10f );
            Seq.AppendCallFunc ( () => { Debug.Log ( "Seq Call Func Started!" ); } );
            Seq.Append ( new Mission_4() );
            Seq.Start();
        }
    }



}
