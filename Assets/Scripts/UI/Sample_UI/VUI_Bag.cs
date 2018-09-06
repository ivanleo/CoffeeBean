using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

/// <summary>
/// 请填写[ 类的作用 ]
/// </summary>
[CUIInfo ( PrefabName = "Resources/UI/Prefab/UI_Bag.prefab", IsSigleton = true, IsAnimationUI = true, Description = " 请填写[ 类的作用 ] " )]
public class VUI_Bag : CUIBase<VUI_Bag>
{
    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Button Button_Button;

    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        Button_Button = transform.FindChildComponent< Button > ( "Button" );
        InitEvent();
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    private void InitEvent()
    {
        Button_Button.onClick.AddListener ( On_Button_Button_Click );
    }

    /// <summary>
    /// Button_Button点击事件
    /// </summary>
    private void On_Button_Button_Click()
    {
        DestroyUI();
    }

}
