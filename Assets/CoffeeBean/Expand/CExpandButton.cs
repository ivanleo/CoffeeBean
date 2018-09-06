using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using UnityEngine.UI;
using UnityEngine;
using DG.Tweening;

namespace CoffeeBean
{
    public static class CExpandButton
    {
        /// <summary>
        /// 延时激活按钮
        /// </summary>
        /// <param name="target"></param>
        public static void DelayToAction( this Button target, float time )
        {
            target.interactable = false;
            Image img = target.GetComponent<Image>();
            img.DOKill( true );
            Color StartColor = img.color;
            img.color = Color.black;
            img.DOColor( StartColor, time ).OnComplete( () => { target.interactable = true; } );
        }
    }
}

