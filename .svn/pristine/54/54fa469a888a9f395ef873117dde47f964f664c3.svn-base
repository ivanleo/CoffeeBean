using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

/// <summary>
/// 请填写[ 类的作用 ]
/// </summary>
[CUIInfo ( PrefabName = "Resources/UI/Prefab/UI_RepItem.prefab", IsSigleton = false, IsAnimationUI = false, Description = " 请填写[ 类的作用 ] " )]
public class VUI_RepItem : CUIBase<VUI_RepItem>, IRepScrollItem
{
    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Text Text_Text;

    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Text Name_Text;

    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        Text_Text = transform.FindChildComponent< Text > ( "Text" );
        Name_Text = transform.FindChildComponent< Text > ( "Name" );
        InitEvent();
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    private void InitEvent()
    {
    }

    /// <summary>
    /// 填充数据
    /// </summary>
    /// <param name="data"></param>
    public void FillData ( IRepScrollItemData data )
    {
        ddt dt = data as ddt;
        Text_Text.text = dt.index.ToString();
        Name_Text.text = dt.name;
    }

}
