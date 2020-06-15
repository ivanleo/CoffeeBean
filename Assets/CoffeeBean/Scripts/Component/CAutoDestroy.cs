using System;
using CoffeeBean;
using UnityEngine;

/// <summary>
/// 自动销毁组件
/// </summary>
public class CAutoDestroy : MonoBehaviour
{
    /// <summary>
    /// 延时销毁的时间
    /// </summary>
    public float delayTime = 1f;

    private void Start()
    {
        Destroy( gameObject, delayTime );
    }
}