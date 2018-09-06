using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using CoffeeBean;

public class Sample_06_EncryptInt : MonoBehaviour
{
    private CEncryptInt _A;
    private CEncryptInt _B = new CEncryptInt ( 10 );
    private CEncryptInt _C = 20;
    private CEncryptInt _D;

    void Start ()
    {
        Debug.Log ( "_A = " + _A );
        Debug.Log ( "_B = " + _B );
        Debug.Log ( "_C = " + _C );
        Debug.Log ( "_D = " + _D );

        _A = new CEncryptInt ( 100 );
        _D = _A + _C;
        _C = _B + 10;
        _B = _A;

        Debug.Log ( "_A DeEncrpty= " + _A.GetDecryptionData() );
        Debug.Log ( "_B DeEncrpty= " + _B.GetDecryptionData() );
        Debug.Log ( "_C DeEncrpty= " + _C.GetDecryptionData() );
        Debug.Log ( "_D DeEncrpty= " + _D.GetDecryptionData() );

        Debug.Log ( "_A ToString = " + _A.ToString() );
        Debug.Log ( "_B ToString= " + _B.ToString() );
        Debug.Log ( "_C ToString= " + _C.ToString() );
        Debug.Log ( "_D ToString= " + _D.ToString() );

        Debug.Log ( "_A Encrpty = " + _A.GetEncryptionData() );
        Debug.Log ( "_B Encrpty= " + _B.GetEncryptionData() );
        Debug.Log ( "_C Encrpty= " + _C.GetEncryptionData() );
        Debug.Log ( "_D Encrpty= " + _D.GetEncryptionData() );

        bool b1 = _A == _B;
        bool b2 = _B >= _C;
        bool b3 = _C != _D;

        Debug.Log ( "b1 = " + b1 );
        Debug.Log ( "b2 = " + b2 );
        Debug.Log ( "b3 = " + b3 );

    }

}
