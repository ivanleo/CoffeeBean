using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

/// <summary>
/// 请填写[ 类的作用 ]
/// </summary>
[CUIInfo ( PrefabName = "Resources/UI/Prefab/UI_2.prefab", IsSigleton = true, IsAnimationUI = true, Description = " 请填写[ 类的作用 ] " )]
public class VUI_2 : CUIBase<VUI_2>
{
    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Button prev_Button;

    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Button next_Button;

    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        prev_Button = transform.FindChildComponent< Button > ( "prev" );
        next_Button = transform.FindChildComponent< Button > ( "next" );
        InitEvent();
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    private void InitEvent()
    {
        prev_Button.onClick.AddListener ( On_prev_Button_Click );
        next_Button.onClick.AddListener ( On_next_Button_Click );
    }

    /// <summary>
    /// prev_Button点击事件
    /// </summary>
    private void On_prev_Button_Click()
    {
        DestroyUI();
    }

    /// <summary>
    /// next_Button点击事件
    /// </summary>
    private void On_next_Button_Click()
    {
        VUI_3.CreateLayoutUI();
    }

}
