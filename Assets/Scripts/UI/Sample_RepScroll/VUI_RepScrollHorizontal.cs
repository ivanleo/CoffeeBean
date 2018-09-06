using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;
using System.Collections.Generic;

/// <summary>
/// 请填写[ 类的作用 ]
/// </summary>
[CUIInfo ( PrefabName = "Resources/UI/Prefab/UI_RepScrollHorizontal.prefab", IsSigleton = true, IsAnimationUI = true, Description = " 请填写[ 类的作用 ] " )]
public class VUI_RepScrollHorizontal : CUIBase<VUI_RepScrollHorizontal>
{
    /// <summary>
    /// 无限滚动组件
    /// </summary>
    private CInfiniteScroll SCR;

    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        SCR = GetComponent<CInfiniteScroll>();
    }

    /// <summary>
    /// 设定数据
    /// </summary>
    /// <param name="datas"></param>
    public void BindData ( List<IRepScrollItemData> datas )
    {
        SCR.Bind ( datas, typeof ( VUI_RepItem ) );
    }
}
