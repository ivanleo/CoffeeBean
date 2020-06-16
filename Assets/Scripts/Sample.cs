using UnityEngine;
using System.Collections;
using CoffeeBean;
using UnityEngine.SceneManagement;

/// <summary>
/// 示例工程入口
/// </summary>
public class Sample : MonoBehaviour
{
    private void OnGUI()
    {
        if ( GUI.Button( new Rect( 100, 100, 200, 30 ), "UI测试" ) )
        {
            gameObject.AddComponent<UITest>();
        }

        if ( GUI.Button( new Rect( 100, 150, 200, 30 ), "场景测试" ) )
        {
            gameObject.AddComponent<SceneTest>();
        }

        if ( GUI.Button( new Rect( 100, 200, 200, 30 ), "任务测试" ) )
        {
            gameObject.AddComponent<TaskTest>();
        }
    }
}

internal class SampleScene : CSceneBase
{
    public override void AfterEnterScene( Scene scene )
    {
    }

    public override void BeforeLeftScene( Scene scene )
    {
    }

    public override void SceneUpdate()
    {
    }
}