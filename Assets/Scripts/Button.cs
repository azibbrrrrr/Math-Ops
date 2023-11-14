using System.Collections;
using System.Collections.Generic;
using UnityEngine;
using UnityEngine.Events;

public class Button : MonoBehaviour
{
    public UnityEvent buttonClick;

    void Awake()
    {
        if (buttonClick == null) { buttonClick = new UnityEvent(); }
    }

    void OnMouseDown()
    {
        if (IsMobilePlatform())
        {
            // Check if it's a touch event on mobile
            if (Input.touchCount > 0 && Input.GetTouch(0).phase == TouchPhase.Began)
            {
                HandleClick();
            }
        }
        else
        {
            // Handle mouse click
            HandleClick();
        }
    }

    void HandleClick()
    {
        buttonClick.Invoke();
    }

    bool IsMobilePlatform()
    {
        return Application.platform == RuntimePlatform.Android || Application.platform == RuntimePlatform.IPhonePlayer;
    }
}
