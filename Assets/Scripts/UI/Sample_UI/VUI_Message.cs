using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;
using DG.Tweening;

/// <summary>
/// 请填写[ 类的作用 ]
/// </summary>
[CUIInfo ( PrefabName = "Resources/UI/Prefab/UI_Message.prefab", IsSigleton = false, IsAnimationUI = true, Description = " 请填写[ 类的作用 ] " )]
public class VUI_Message : CUIBase<VUI_Message>
{
    /// <summary>
    /// 显示UI
    /// </summary>
    /// <param name="content"></param>
    public static void Show ( string content )
    {
        var ui = CreateUI();
        ui.Text_Text.text = content;
    }

    /// <summary>
    /// 请填写[ 成员意义 ]
    /// </summary>
    private Text Text_Text;

    /// <summary>
    /// 苏醒
    /// </summary>
    private void Awake()
    {
        Text_Text = transform.FindChildComponent< Text > ( "Image/Text" );
        InitEvent();

        rectTransform.DOLocalMoveY ( 500, 3f ).SetRelative ( true ).OnComplete ( () => { DestroyUI ( this ); } );
    }

    /// <summary>
    /// 初始化事件
    /// </summary>
    private void InitEvent()
    {
    }

}
