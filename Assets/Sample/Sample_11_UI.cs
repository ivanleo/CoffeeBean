using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;


public class Sample_11_UI : MonoBehaviour
{
    private void OnGUI()
    {
        Rect rt = new Rect ( 20, 20, 400, 60 );
        var gs = GUI.skin.button;
        gs.fontSize = 28;

        if ( GUI.Button ( rt, "显示背包", gs ) )
        {
            VUI_Bag.CreateUI();
        }


        rt.y += 80f;
        if ( GUI.Button ( rt, "飘字吧", gs ) )
        {
            VUI_Message.Show ( "哈哈哈" );
        }


        rt.y += 80f;
        if ( GUI.Button ( rt, "创建层级UI", gs ) )
        {
            VUI_1.CreateLayoutUI();
        }

    }

}
