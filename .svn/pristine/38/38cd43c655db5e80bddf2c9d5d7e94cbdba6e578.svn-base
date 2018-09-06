using CoffeeBean;
using System.Collections;
using System.Collections.Generic;
using UnityEngine;

public class Sample_05_C1: CSingleton<Sample_05_C1>
{
    public int _A;
    public Sample_05_C1()
    {
        _A = 10;
        Debug.Log ( "Sample_05_C1 构造 _A = " + _A );
    }

}

public class Sample_05_C2 : CSingletonMono<Sample_05_C2>
{
    public int _B;
    private void Awake()
    {
        _B = 100;
        Debug.Log ( "Sample_05_C2 Awake _B = " + _B );
    }

    private void Start()
    {
        _B = 200;
        Debug.Log ( "Sample_05_C2 Start _B = " + _B );
    }
}



public class Sample_05_Singleton : MonoBehaviour
{

    // Use this for initialization
    void Start ()
    {
        Debug.Log ( Sample_05_C1.Instance._A );
        Sample_05_C1.Instance._A = 111;
        Debug.Log ( Sample_05_C1.Instance._A );

        Debug.Log ( Sample_05_C2.Instance._B );
        Sample_05_C2.Instance._B = 222;
        Debug.Log ( Sample_05_C2.Instance._B );
    }
}
