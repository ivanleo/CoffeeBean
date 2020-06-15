using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 自动旋转
/// </summary>
public class CRotateForver : MonoBehaviour
{
    // 每秒转动角度数
    public float RotateSpeed = 12f;
    void Update()
    {
        transform.localEulerAngles += Vector3.forward * RotateSpeed * Time.deltaTime;
    }
}
