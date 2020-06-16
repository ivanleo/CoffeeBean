//
// This is auto generate by UICreator
// Please don't change it manual except you know what you do
// Any question you can contact QQ:6828000
//
// Author: Leo
//

using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

/// <summary>
/// 排行榜界面
/// </summary>
[CUIBind ( Prefab = "Assets/Prefab/UI/UI_Rank.prefab" )]
public partial class UI_Rank : CUIMuti<UI_Rank>
{
    /// <summary>
    /// 内容区域 RectTransform
    /// </summary>
    private RectTransform Content_RectTransform;
    /// <summary>
    /// 退出按钮 Button
    /// </summary>
    private Button Button_Button;
    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        Content_RectTransform = transform.FindChildComponent<RectTransform> ( "Scroll View/Viewport/Content" );
        Button_Button = transform.FindChildComponent<Button> ( "Button" );

        InitEvents();

    }

    /// <summary>
    /// 事件注册
    /// </summary>
    private void InitEvents()
    {
        Button_Button.onClick.AddListener(On_Button_Button_Click);
    }

} // UI Define Class End
