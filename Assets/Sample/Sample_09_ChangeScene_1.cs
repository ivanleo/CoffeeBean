using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;

public class Sample_09_ChangeScene_1 : MonoBehaviour
{
    private Button b_1 = null;
    private Button b_2 = null;

    void Awake()
    {
        CLOG.I ( "Sample_09_ChangeScene_1 Awake" );
    }

    // Use this for initialization
    void Start ()
    {
        CLOG.I ( "Sample_09_ChangeScene_1 Start" );
        b_1 = transform.FindChildComponent<Button> ( "Button" );
        b_2 = transform.FindChildComponent<Button> ( "Button (1)" );

        b_1.onClick.AddListener ( OnBtn_1_Click );
        b_2.onClick.AddListener ( OnBtn_2_Click );
    }

    private void OnBtn_1_Click()
    {
        CSceneManager.Instance.ChangeSceneImmediately ( "Sample_02", () =>
        {
            CLOG.I ( "Sample_2 Load complete" );
        } );
    }

    private void OnBtn_2_Click()
    {
        CSceneManager.Instance.ChangeScene ( "Sample_02", null, () =>
        {
            CLOG.I ( "Sample_2 Load complete" );
        } );
    }

    private void OnDestroy()
    {
        CLOG.I ( "Sample_09_ChangeScene_1 Destroy" );
    }
}
