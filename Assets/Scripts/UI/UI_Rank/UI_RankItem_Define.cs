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
/// 排行项
/// </summary>
[CUIBind ( Prefab = "Assets/Prefab/UI/UI_RankItem.prefab", IsModel = false, BackColor = "#00000099" )]
public partial class UI_RankItem : CUISingle<UI_RankItem>
{
    /// <summary>
    /// 序号 Text
    /// </summary>
    private Text index_Text;
    /// <summary>
    /// 名字 Text
    /// </summary>
    private Text name_Text;
    /// <summary>
    /// 战斗力 Text
    /// </summary>
    private Text combat_Text;
    /// <summary>
    /// 挑战按钮 Button
    /// </summary>
    private Button fight_Button;
    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        index_Text = transform.FindChildComponent<Text> ( "index" );
        name_Text = transform.FindChildComponent<Text> ( "name" );
        combat_Text = transform.FindChildComponent<Text> ( "combat" );
        fight_Button = transform.FindChildComponent<Button> ( "fight" );

        InitEvents();

    }

    /// <summary>
    /// 事件注册
    /// </summary>
    private void InitEvents()
    {
        fight_Button.onClick.AddListener(On_fight_Button_Click);
    }

} // UI Define Class End
