using UnityEngine;
using UnityEngine.UI;
using CoffeeBean;
using System;
using UnityEngine.EventSystems;

public class Sample_07_TouchEvent : MonoBehaviour
{

    private Button m_Button = null;
    private Image m_Image = null;

    // Use this for initialization
    void Start ()
    {
        m_Button = transform.FindChildComponent<Button> ( "Button" );
        m_Image = transform.FindChildComponent<Image> ( "Image" );

        m_Button.GetTouchEventModule().OnTouchDown += TouchBegan;
        m_Image.GetTouchEventModule().OnTouchDown += TouchBegan;

        m_Button.GetTouchEventModule().OnTouchEnter += TouchEnter;
        m_Image.GetTouchEventModule().OnTouchEnter += TouchEnter;

        m_Button.GetTouchEventModule().OnTouchExit += TouchExit;
        m_Image.GetTouchEventModule().OnTouchExit += TouchExit;

        m_Button.GetTouchEventModule().OnTouchUp += TouchEnd;
        m_Image.GetTouchEventModule().OnTouchUp += TouchEnd;
    }

    private void TouchEnd ( PointerEventData eventData )
    {
        Debug.Log ( "Touch end on " + eventData.pointerEnter.name );
    }

    private void TouchExit ( PointerEventData eventData )
    {
        Debug.Log ( "Touch exit on " + eventData.pointerEnter.name );
    }

    private void TouchEnter ( PointerEventData eventData )
    {
        Debug.Log ( "Touch enter on " + eventData.pointerEnter.name );
    }

    private void TouchBegan ( PointerEventData eventData )
    {
        Debug.Log ( "Touch began on " + eventData.pointerEnter.name );
    }
}
