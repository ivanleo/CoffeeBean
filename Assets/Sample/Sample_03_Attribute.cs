using CoffeeBean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

/// <summary>
/// 测试枚举
/// </summary>
public enum ETest
{
    [CEnumDesc ( "你好" )]
    HELLO,
    [CEnumDesc ( "操" )]
    FUCK,
    [CEnumDesc ( "啦啦啦" )]
    LALALA,
}

public class Sample_03_Attribute : MonoBehaviour
{
    [CShowEnumDesc ( "测试枚举" )]
    public ETest m_Test_1 = ETest.HELLO;

    public ETest m_Test_2 = ETest.HELLO;

    [ReadOnly]
    public ETest m_Test_ReadOnly = ETest.HELLO;

    // Use this for initialization
    void Start ()
    {

    }

    // Update is called once per frame
    void Update ()
    {

    }
}
