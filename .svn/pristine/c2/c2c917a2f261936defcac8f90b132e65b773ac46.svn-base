using UnityEngine;
using CoffeeBean;

public class Sample_08_CustomMessage_Sender : MonoBehaviour, IMsgSender
{
    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 300, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "发送消息", gs ) )
        {
            this.DispatchMessage ( ECustomMessageType.SAMPLE_EVENT, 123, "哈哈", true, 333.33f );
        }
    }
}
