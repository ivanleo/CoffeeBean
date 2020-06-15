using System;
using UnityEngine;
using UnityEngine.UI;

namespace CoffeeBean
{
    /// <summary>
    /// Toggle文字颜色设置器
    /// 用于toggle on,off 时文字显示不同颜色
    /// </summary>
    [RequireComponent( typeof( Toggle ) )]
    [ExecuteInEditMode]
    public class CToggleTextColorSwitch : MonoBehaviour
    {
        /// <summary>
        /// 未开启的颜色
        /// </summary>
        public Color OffColor;

        /// <summary>
        /// 开启的颜色
        /// </summary>
        public Color OnColor;

        private Text childText;
        private Toggle toggle;

        private void Awake()
        {
            childText = GetComponentInChildren<Text>();
            if ( childText != null )
            {
                toggle = GetComponent<Toggle>();
                toggle.onValueChanged.AddListener( Refresh );
            }

            Refresh( false );
        }

        /// <summary>
        /// 刷新
        /// </summary>
        private void Refresh( bool isOn )
        {
            childText.color = toggle.isOn ? OnColor : OffColor;
        }
    }
}