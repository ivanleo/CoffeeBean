using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

public class ddt : IRepScrollItemData
{
    public int index;
    public string name;
}


public class Sample_13_RepScroll : MonoBehaviour
{
    private void Awake()
    {
        List<IRepScrollItemData> datas =  new List<IRepScrollItemData> ( 30 );

        for ( int i = 0; i < datas.Capacity; i++ )
        {
            ddt o = new ddt();
            o.index = i + 1;
            o.name = new string ( new char[] { ( char ) CMath.Rand ( 128 ), ( char ) CMath.Rand ( 128 ), ( char ) CMath.Rand ( 128 ), ( char ) CMath.Rand ( 128 ) } );
            datas.Add ( o );
        }


        var ui = VUI_RepScroll.CreateUI();
        ui.BindData ( datas );

        var uihor = VUI_RepScrollHorizontal.CreateUI();
        uihor.BindData ( datas );
    }

}